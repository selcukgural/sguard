<div align="center">
<img src="icon.png" alt="SGuard" width="205" height="81"/>
</div>

`SGuard` is a lightweight guard library for .NET that lets you express preconditions clearly and reliably.  
Use Is.* for boolean checks and ThrowIf.* for throwing guards when conditions are met.  
The GuardCallback/GuardOutcome model brings explicit feedback semantics, and CallerArgumentExpression enriches exception messages with call-site expressions.

<strong>Highlights</strong>
- **Consistent API:** Is.* (bool) and ThrowIf.* (throw) follows the same outcome rule
- **Clear feedback:** GuardCallback(GuardOutcome) — explicit Success / Failure
- **Diagnostics-friendly:** exceptions include call-site expressions and actual values
- **Practical NullOrEmpty:** string, collections, arrays, nullable, common value types
- **Production-ready:** input validation, finally based callbacks, comprehensive tests

<strong>Outcome rules</strong>
- `Is.*`: true ⇒ Success, false ⇒ Failure
- `Is.NullOrEmpty`: true (null/empty) ⇒ Failure, false ⇒ Success
- `ThrowIf.*`: condition met (throws) ⇒ Failure, not met ⇒ Success

<strong>Note (Breaking Changes)</strong>  
This release redesigns the callback API with GuardCallback/GuardOutcome and standardizes naming (e.g., GreaterThanOrEqualException).


## Quick Start
Install from NuGet (example package id; adjust to your actual PackageId):
- dotnet add package SGuard

Basic usage:

```csharp
using SGuard;

// Boolean checks
if (Is.Between(age, 18, 65))
{
// ...
}

// Throwing guards
ThrowIf.GreaterThan(order.Quantity, order.Stock);

// Callback usage
Is.GreaterThan(x, y, GuardCallbacks.OnSuccess(() => Log("x > y")));

// Rich exception messages
ThrowIf.Between(value, min, max);

// throws when name is null or empty
ThrowIf.NullOrEmpty(user.Name, GuardCallbacks.OnFailure(() => Log("name missing")));
bool isEmpty = Is.NullOrEmpty(tags, GuardCallbacks.OnSuccess(() => Log("tags available")));

// throws when quantity > stock
ThrowIf.GreaterThan(order.Quantity, order.Stock, GuardCallbacks.OnFailure(() => Log("invalid stock")));

```
---
## Contents
- What’s new and Breaking Changes
- Quick Start
- How Guards Work
- Callback Model (GuardCallback / GuardOutcome)
- NullOrEmpty Semantics
- API Naming Conventions
- Exception Messages (CallerArgumentExpression)
- Error Handling Guarantees
- Performance and AOT/Trimming Notes
- Versioning and Migration Guide
- Install
- License and Contributing

---
## What’s new and Breaking Changes
This release introduces a redesigned callback API and naming consistency improvements. These are breaking changes.
Must-read-breaking changes:
1. Callback redesign

- Old: Callback with OnFailure/OnSuccess and internal InvokeCallback(bool)
- New: GuardCallback delegate receiving GuardOutcome (Success/Failure)
- Helpers: GuardCallbacks.OnSuccess(Action), GuardCallbacks.OnFailure(Action)
- All Is.* and ThrowIf.* signatures changed from Callback? to GuardCallback?

1. Outcome semantics are explicit and unified

- Is.*: result == true -> Success, result == false -> Failure
- Is.NullOrEmpty: true (empty) → Failure, false → Success
- ThrowIf.*: condition met (exception thrown) → Failure, condition not met → Success

1. Exception naming consistency

- GreaterOrEqualThanException has been renamed to GreaterThanOrEqualException
- Throw/ThrowIf methods are aligned with exception names

1. Exception messages enhanced

- Exceptions now capture real call-site expressions with CallerArgumentExpression and embed them in Message and Exception.Data

1. Possible behavioral differences for NullOrEmpty

- Complex types that are non-null and have no readable properties are NOT considered empty
- See `NullOrEmpty` Semantics for details and warnings

If you are upgrading from older versions, read the Migration Guide below carefully.

---

## How Guards Work
- `Is.*` methods return bool and optionally invoke a callback with the evaluation outcome.
- `ThrowIf.*` methods throw an exception when the specified condition is met; otherwise, they return normally. They also optionally invoke a callback with the evaluation outcome.

Outcome rule:
- Is.*: true ⇒ Success, false ⇒ Failure
- Is.NullOrEmpty: true (null or empty) ⇒ Failure, false ⇒ Success
- ThrowIf.*: condition met (throw) ⇒ Failure, no throw ⇒ Success

---

## Callback Model (GuardCallback / GuardOutcome)
New, explicit, and simple callback system.
- GuardOutcome enum: Success | Failure
- GuardCallback delegate: public delegate void GuardCallback(GuardOutcome outcome);
- Helpers:
    - GuardCallbacks.OnSuccess(Action action)
    - GuardCallbacks.OnFailure(Action action)

Examples:

```csharp
// Success callback when Is.* returns true
Is.GreaterThan(10, 5, GuardCallbacks.OnSuccess(() => Console.WriteLine("ok")));

// Failure callback when Is.* returns false
Is.LessThan(2, 1, GuardCallbacks.OnFailure(() => Console.WriteLine("failed")));

// ThrowIf.*: exception path is Failure, non-exception path is Success
ThrowIf.GreaterThan(5, 1, GuardCallbacks.OnFailure(() => Console.WriteLine("will throw before this line")));
```

Why this is better:
- Explicit semantics, no inversions/hacks
- Same outcome rules across Is.* and ThrowIf.*

Policy on callback exceptions:
- If your callback throws, that exception will propagate. Documented behavior by design. Keep callback code safe.

---
## Outcome Summary Table
- Is.GreaterThan / Is.LessThan / Is.Between: result true ⇒ Success; result false ⇒ Failure
- Is.NullOrEmpty: result true (is null or empty) ⇒ Failure; result false ⇒ Success
- ThrowIf.* guards: condition met (throw) ⇒ Failure; condition not met ⇒ Success

---

## NullOrEmpty Semantics
Explicit rules:
- string: null or string.Empty ⇒ empty (Failure)
- Array: Length == 0 ⇒ empty
- ICollection/IReadOnlyCollection/IDictionary/IReadOnlyDictionary: Count == 0 ⇒ empty
- IEnumerable (no Count): treated as empty if it yields no elements. Note: single-use enumerable might be consumed for the check; document usage accordingly
- Nullable: HasValue == false => empty
- Non-null complex types:
    - Objects without readable properties are NOT considered empty
    - Generally, if it’s a plain reference type and not null, it’s NOT empty

- Value types like numeric, bool, Guid, DateTime follow “empty” conventions used internally (e.g., zero, Guid.Empty, ticks == 0, min value, etc.)

Caveat:
- For IEnumerable without Count, checking emptiness may consume the sequence. Avoid passing single-use iterators if you need to iterate afterward.

---

## API Naming Conventions
We standardized comparison exception names and Throw methods to match:
- GreaterThanException
- GreaterThanOrEqualException
- LessThanException
- LessThanOrEqualException
- BetweenException

Ensure your code references the new GreaterThanOrEqualException (previously named GreaterOrEqualThanException).

---
## Exception Messages (CallerArgumentExpression)
All guard exceptions (e.g., BetweenException, GreaterThanException, LessThanException, GreaterThanOrEqualException, NullOrEmptyException) embed call-site expressions and actual values:
- Message includes expressions (e.g., "value", "min", "user.Age", "limit + 5") and actual runtime values
- Exception.Data includes keys like valueExpr, minExpr, leftExpr, rightExpr, etc.

This makes diagnostics significantly easier.

---
## Error Handling Guarantees
- All guards validate inputs and will throw ArgumentNullException when necessary.
- Callbacks are invoked in final blocks to ensure execution regardless of early returns.
- If a callback throws, that exception will propagate (documented behavior)


---
## Performance and AOT/Trimming Notes
- Reflection and Expression.Compile are used in some NullOrEmpty scenarios (e.g., expression-based selection).
- Recommend testing with trimming/AOT if your application uses NativeAOT or aggressive linkers.
- If needed, mark dynamic parts with suitable attributes or document APIs as “**not AOT-safe.**”

Performance tips:
- Prefer collections with Count/Length for O(1) checks.
- Avoid passing single-use IEnumerable to NullOrEmpty if you need to iterate after checks.


---
## Versioning and Migration Guide
This release is a major version with breaking changes.
1. Callback API
- Old: Callback OnFailure/OnSuccess, InvokeCallback(bool)
- New: GuardCallback(GuardOutcome), GuardCallbacks.OnSuccess/OnFailure
- Migrate:
    - Replace Callback with GuardCallback
    - Replace Callback.OnSuccess(Action) with GuardCallbacks.OnSuccess(Action)
    - Replace Callback.OnFailure(Action) with GuardCallbacks.OnFailure(Action)
    - Outcome rules are now explicit as described above

1. Naming consistency
- Replace GreaterOrEqualThanException with GreaterThanOrEqualException
- Update any Throw.* method names accordingly (Throw.GreaterThanOrEqualException)

1. NullOrEmpty behavior
- Plain reference types without properties and non-null are NOT empty
- If previous behavior considered such objects empty, refactor your checks

1. Exception messages
- Now richer via CallerArgumentExpression; no action required but expect different messages and Data content

**Recommended upgrade steps:**
- Update package
- Fix compile errors around removed a Callback type and renamed exception
- Review callback site logic; use GuardCallbacks helpers
- Re-run tests and check exception messages
- If you rely on expression-based NullOrEmpty, verify trimming/AOT scenarios

