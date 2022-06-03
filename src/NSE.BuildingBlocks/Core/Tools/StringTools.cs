namespace Core.Tools;

public static class StringTools
{
    public static string ApenasNumeros(this string str, string input)
        => new string(input.Where(char.IsDigit).ToArray());
}
