﻿namespace ns2x.Model.Semantic;

public interface IValue : ISemanticNode
{
    Range SourceRange { get; }
}