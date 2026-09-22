# Scratch C# Demos for 3D Game Engine Development

A small, deliberately over-commented C# console project used to teach the object-oriented
fundamentals that Unity and MonoGame assume you already know. 

---

## Contents

| File | Purpose |
| --- | --- |
| `Vector3.cs` | A 3D vector. Shallow vs deep copy, operator overloading, the `Equals`/`GetHashCode`/`==` contract. |
| `ColorRGBA.cs` | A four-channel colour. Property validation, arithmetic operators, static factory properties, interpolation. |
| `Player.cs` | A game actor. Encapsulation, derived (computed) properties, deep-copying an object that contains another object. |
| `GDMath.cs` | Static maths helpers. Two different clamping strategies and when each is appropriate. |
| `Program.cs` | Ten self-contained demonstration methods, one per concept, called in turn from `Main`. |

---

## Concepts demonstrated

**Value types vs reference types.** Assigning one `Vector3` variable to another copies the
*reference*, not the object. Two variables then point at one object on the heap, and a change through
either is visible through both. `ShallowCopy` makes this explicit; `DeepCopy` produces a genuinely
independent object. Note that `UnityEngine.Vector3` is a `struct`, so it copies by value — this
project uses a `class` precisely so the distinction has teeth.

**Deep copying a composed object.** `Player` holds a `Vector3`. Copying the player's fields alone
would leave both players sharing one position object, so moving one moves the other. `Player.DeepCopy`
recurses into `Vector3.DeepCopy`. This is the Prototype pattern in miniature.

**Encapsulation with validating setters.** `Player.Health` refuses negative values, `Player.ActorType`
refuses null and empty strings, `Player.Position` refuses null, and every `ColorRGBA` channel is range
checked. Crucially, the *constructors assign through the properties* rather than writing to the private
fields, so arguments are validated at construction time too. Writing to fields directly in a constructor
is a common way to produce an object that is born in a state its own setters would have rejected.

**Derived state instead of duplicated state.** `Player.IsAlive` is a read-only computed property over
`Health`. There is no `isAlive` field, so there is nothing to keep in sync and no way for the two to
disagree.

**Operator overloading.** `Vector3` supports `+ - * /` with vectors and scalars; `ColorRGBA` supports
`+ - *`. Note the mirrored scalar overloads — C# matches operators on the static types of *both*
operands, so `10 * v` needs a separate overload from `v * 10`.

**The equality contract.** If you override `Equals`, you must override `GetHashCode`, and both must be
derived from the same fields. Omitting `GetHashCode` raises compiler warning CS0659 and silently breaks
`Dictionary` and `HashSet` lookups: two objects that compare equal land in different buckets and neither
can find the other. `Vector3` and `ColorRGBA` additionally overload `==` and `!=`, which C# requires to
be declared as a pair. Inside those overloads, null is tested with `ReferenceEquals` — writing
`v1 == null` there would call the operator recursively until the stack overflows.

**Reference equality vs value equality.** `Player` deliberately does *not* overload `==`. Run
`DemoPlayerReferenceVsValueEquality` to see `p4.Equals(p1)` return `true` while `p4 == p1` returns
`false` for the same pair of objects.

---

