# AGENTS Instructions

## Test Creation Skill
- When creating or updating tests, follow `skills/test-generator/SKILL.md`.
- Use factory classes and inherit from `BaseTests<T>`.
- Keep test names in `[Method]_[Context]_[ExpectedResult]` format.
- Organize tests in partial classes split by method and overload.
- Use fixtures for `TestItem`, `TestEnumItem`, and `TestStructItem`.

## Common Test Namespaces
- Keep common test classes in `TheChest.Tests.Common`.
- Keep test-only extension methods `internal` in `TheChest.Tests.Common.Extensions`.
