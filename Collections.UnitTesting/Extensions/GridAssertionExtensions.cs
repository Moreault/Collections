namespace ToolBX.Collections.UnitTesting.Extensions;

// AssertBox can only offer predicate-based Contain/NotContain through concrete receiver
// overloads (the element type can't be inferred from an implicit lambda on an arbitrary
// IEnumerable<T>, and Assertions<T> is invariant). Grid<T> and OverlapGrid<T> live in
// ToolBX.Collections.Grid, which AssertBox must not reference, so their overloads belong here.
public static class GridAssertionExtensions
{
    public static Assertions<Grid<T>> Contain<T>(this Assertions<Grid<T>> a, Func<Cell<T>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        Fail.When(
            !a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
        return a;
    }

    public static Assertions<Grid<T>> NotContain<T>(this Assertions<Grid<T>> a, Func<Cell<T>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        Fail.When(
            a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "not to contain an element matching the predicate", a.Subject));
        return a;
    }

    public static Assertions<OverlapGrid<T>> Contain<T>(this Assertions<OverlapGrid<T>> a, Func<Cell<T>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        Fail.When(
            !a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
        return a;
    }

    public static Assertions<OverlapGrid<T>> NotContain<T>(this Assertions<OverlapGrid<T>> a, Func<Cell<T>, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        Fail.When(
            a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "not to contain an element matching the predicate", a.Subject));
        return a;
    }
}
