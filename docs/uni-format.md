# Uni file format

Uni is a simple data format used for storing configuration parameters, application settings, and other metadata. The Uni format supports a hierarchical structure of data and consists of a set of key-value pairs separated by the `=` symbol.

## Base Syntax

Each Uni element consists of a key-value pair, where the key is a string composed of letters, digits, and underscores, and the value can be any string that does not contain a line break. The key and value are separated by the `=` symbol. Each Uni element must be on a separate line.

Example:

```properties
db.host=127.0.0.1
db.port=5432
db.name=example
```

## Hierarchy

Uni supports a hierarchical structure of data. To create a hierarchy, dots are used between keys. For example, the following Uni file describes the settings hierarchy for an application:

```properties
database.host=127.0.0.1
database.port=5432
database.name=example
app.title=My App
app.version=1.0
```

Here, the first three parameters belong to the database settings, and the last two belong to the application settings.

## Comments

Uni supports comments, which can be placed at the end of a line or on a line by themselves. Comments start with the `#` symbol and continue to the end of the line. Comments are ignored when reading the file.

Example:

```text
# Database settings
db.host=127.0.0.1 # The IP address of the database server
db.port=5432
db.name=example
```

## References

Uni supports the use of references in the value of a parameter. References are represented using the `${parameter_key}` format, where `parameter_key` is the key of the parameter being referenced. This format allows for the dynamic referencing of other parameters within the Uni file.

Example:

```properties
db.host=127.0.0.1
db.port=5432
db.name=example
db.connectionString=jdbc:mysql://${db.host}:${db.port}/${db.name}
```

In this example, the `db.url` parameter uses the `${db.host}`, `${db.port}` and `${db.name}` references to dynamically construct the connection string.

## Attributes

Uni supports the use of attributes on both parameters and scopes. Attributes provide additional information about a parameter or scope, such as whether it is sensitive or the output format. Attributes are indicated by square brackets `[]` after the parameter or scope name, followed by the attribute name and its value, separated by the `=` symbol.

Example:

```properties
db[output]=db.json
db.password[secret]=true
```

In this example, the `db` scope has an `output` attribute with the value `db.json`, and the `db.password` parameter has a `secret` attribute with the value `true`.

Attributes can be used to provide metadata for various Uni processing tools, such as formatters and other utilities that work with the Uni format.

It's important to note that attributes cannot be used as parameter references. However, attribute values can contain references to parameter values.

## Escaping

To include special characters in parameter values, they must be escaped using the character duplication technique. The following characters must be escaped:

```text
. = $ { } [ ] @ ! ( ) " '
```

To escape a character, it must be duplicated one or more times. For example, to include the "=" character in a parameter value, it must be written twice:

```properties
db.user=joe@@example
```

In this case, the value will contain only one `@` character.

Similarly, to include the `$` or `{` character, it must be written twice:

```properties
service.url=http://localhost:${port}/$${{env}}
```

If a duplicated character needs to be included in the value of an attribute, it must also be escaped, for example:

```properties
db.password[secret]=""true""
```

## Case Sensitivity

Uni keys are case sensitive, i.e., keys `db.host` and `DB.HOST` are treated as different keys.

## File Extension

The Uni format uses the file extension `.uni` to indicate that a file follows the Uni format.

Note that while the `.uni` extension is the recommended extension for Uni files, it is not required. Other extensions may be used, but using the `.uni` extension helps ensure consistency and clarity in file naming conventions.

## File Encoding

The Uni format is designed to be encoding-agnostic, meaning that it can be used with any character encoding. However, to ensure maximum compatibility and ease of use, we recommend that Uni files be encoded using UTF-8.

UTF-8 is a widely-used character encoding that supports all Unicode characters and is compatible with a wide range of platforms and software applications. Using UTF-8 encoding for Uni files helps ensure that they can be easily shared and edited by users around the world.

If you are working with Uni files and are unsure about the encoding, check the file's metadata or consult with the file's creator to determine the correct encoding to use.

## Conclusion

The Uni format is a simple but powerful format for storing and transmitting configuration data. It supports hierarchy, references, and attributes, which provide additional information about parameters and scopes.