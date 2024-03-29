﻿using unicfg.Base.SyntaxTree.Values;
using unicfg.Base.SyntaxTree.Walkers;

namespace unicfg.Evaluation.Walkers;

internal sealed class EmitValueBuilder : AsyncWalker
{
    private readonly CancellationToken _cancellationToken;
    private readonly IReadOnlyDictionary<SymbolRef, EmitValue> _dependencies;

    private bool _hasErrors;
    private StringRef _value;

    public EmitValueBuilder(
        IReadOnlyDictionary<SymbolRef, EmitValue> dependencies,
        CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _dependencies = dependencies;
        _cancellationToken = cancellationToken;

        UnresolvedDependency = SymbolRef.Null;
    }

    public SymbolRef UnresolvedDependency { get; private set; }

    public EmitValue? GetResult()
    {
        if (_hasErrors)
        {
            return EmitValue.Error;
        }

        if (UnresolvedDependency != SymbolRef.Null)
        {
            return null;
        }

        return EmitValue.CreateEvaluatedValue(_value);
    }

    public override ValueTask Visit(TextValue textValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (_hasErrors)
        {
            return ValueTask.CompletedTask;
        }

        if (UnresolvedDependency != SymbolRef.Null)
        {
            return ValueTask.CompletedTask;
        }

        _value += textValue.Text;
        return ValueTask.CompletedTask;
    }

    public override ValueTask Visit(ErrorValue errorValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        _hasErrors = true;
        _value = StringRef.Empty;
        return ValueTask.CompletedTask;
    }

    public override ValueTask Visit(RefValue refValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (_hasErrors)
        {
            return ValueTask.CompletedTask;
        }

        if (UnresolvedDependency != SymbolRef.Null)
        {
            return ValueTask.CompletedTask;
        }

        if (!_dependencies.TryGetValue(refValue.Property, out var value))
        {
            UnresolvedDependency = refValue.Property;
            _value = StringRef.Empty;
            return ValueTask.CompletedTask;
        }

        if (value.State is EvaluationState.Error)
        {
            _hasErrors = true;
            _value = StringRef.Empty;
            return ValueTask.CompletedTask;
        }

        _value += value.Value;
        return ValueTask.CompletedTask;
    }
}
