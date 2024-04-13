## Coding Standards

### Code Style

Code should be formatted using standard C# formatting guidelines as enforced by Rider.

Identifier naming scheme:

| Concept                                  | Naming scheme                      | Example identifier       |
|------------------------------------------|------------------------------------|--------------------------|
| Namespace                                | PascalCase                         | `CommunityToolkit`       |
| Class                                    | PascalCase                         | `MainViewModel`          |
| Interface                                | PascalCase with `I` prefx          | `INotifyPropertyChanged` |
| Methods                                  | PascalCase                         | `OnPropertyChanged`      |
| Private methods                          | camelCase with `_` prefix          | `_fooImpl`               |
| Local variables, method parameters       | camelCase                          | `propertyName`           |
| Boolean fields, methods, properties      | must start with `Is`/`is`[^1]      | `IsEmpty`, `isEmpty`     |
| Constants, `readonly` fields, Properties | PascalCase                         | `Greeting`               |
| Exception                                | PascalCase with `Exception` suffix | `DivideByZeroException`  | 

[^1]: Follow the normal rules for these otherwise, choose prefix based on rules for this identifier.

### Git

When working with git, follow these rules:
* DO name your branches descriptive names. You MAY prefix your branch name with `<yourname>/`
* DO NOT end your commit titles in `.`
* DO write commit titles in the imperative mood
* DO NOT go over 72 characters in a line in a commit title or the commit message. URLs are an exception, but they SHOULD be placed at the end of the commit message.
* DO prefix your commit title by the name of the module you're working on. You MAY include the submodule if it is relevant. If a commit touches multiple modules, separate their names by a `+`.
* DO make sure every commit builds on its own.
* TRY to not touch many modules in the same commit.
* AVOID commit titles such as "Fix bug", "Refactor code".