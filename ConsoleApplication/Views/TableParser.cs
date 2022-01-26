using System.Text;

namespace ConsoleApplication.Views;

public static class TableParser
{
  public static string ToStringTable<T>(
    this IEnumerable<T> values,
    string[] columnHeaders,
    params Func<T, object>[] valueSelectors)
  {
    return ToStringTable(values.ToArray(), columnHeaders, valueSelectors);
  }

  private static string ToStringTable<T>(
    this IReadOnlyList<T> values,
    string[] columnHeaders,
    params Func<T, object>[] valueSelectors)
  {

    var arrValues = new string[values.Count + 1, valueSelectors.Length];

    // Fill headers
    for (var colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
    {
      arrValues[0, colIndex] = columnHeaders[colIndex];
    }

    // Fill table rows
    for (var rowIndex = 1; rowIndex < arrValues.GetLength(0); rowIndex++)
    {
      for (var colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
      {
        arrValues[rowIndex, colIndex] = valueSelectors[colIndex]
          .Invoke(values[rowIndex - 1]).ToString() ?? string.Empty;
      }
    }

    return ToStringTable(arrValues);
  }

  private static string ToStringTable(this string[,] arrValues)
  {
    var maxColumnsWidth = GetMaxColumnsWidth(arrValues);
    var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

    var sb = new StringBuilder();
    for (var rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
    {
      for (var colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
      {
        // Print cell
        var cell = arrValues[rowIndex, colIndex];
        cell = cell.PadRight(maxColumnsWidth[colIndex]);
        sb.Append(" | ");
        sb.Append(cell);
      }

      // Print end of line
      sb.Append(" | ");
      sb.AppendLine();

      // Print splitter
      if (rowIndex == 0)
      {
        sb.AppendFormat(" |{0}| ", headerSpliter);
        sb.AppendLine();
      }
    }

    return sb.ToString();
  }

  private static int[] GetMaxColumnsWidth(string[,] arrValues)
  {
    var maxColumnsWidth = new int[arrValues.GetLength(1)];
    for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
    {
      for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
      {
        int newLength = arrValues[rowIndex, colIndex].Length;
        int oldLength = maxColumnsWidth[colIndex];

        if (newLength > oldLength)
        {
          maxColumnsWidth[colIndex] = newLength;
        }
      }
    }

    return maxColumnsWidth;
  }
}