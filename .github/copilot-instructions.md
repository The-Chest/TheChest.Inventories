# Copilot Instructions

When generating or updating tests for this repository, follow the rules in `skills/test-generator/SKILL.md`.

Key reminders:
- Use factory classes and inherit `BaseTests<T>`.
- Keep test methods ordered by production execution flow.
- Use partial test classes split by method/overload.
- Follow naming pattern `[Method]_[Context]_[ExpectedResult]`.
- Use `TestFixture` for `TestItem`, `TestEnumItem`, and `TestStructItem`.
- Keep common classes in `TheChest.Tests.Common` and test-only extensions as `internal` in `TheChest.Tests.Common.Extensions`.
