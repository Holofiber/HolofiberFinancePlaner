namespace FinancialPlanner.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || !input.Contains('@'))
        {
            throw new ArgumentException("Invalid email format.", nameof(input));
        }

        return new Email(input.Trim().ToLowerInvariant());
    }

    public override string ToString() => Value;
}
