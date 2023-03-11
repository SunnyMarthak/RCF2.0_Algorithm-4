using System.Data;
using System.Text.RegularExpressions;
long ConvertToNumbers(string numberString)
{
    Dictionary<string, long> numberTable = new Dictionary<string, long>
    {
        {"zero",0},{"one",1},{"two",2},{"three",3},{"four",4},{"five",5},{"six",6},
        {"seven",7},{"eight",8},{"nine",9},{"ten",10},{"eleven",11},{"twelve",12},
        {"thirteen",13},{"fourteen",14},{"fifteen",15},{"sixteen",16},{"seventeen",17},
        {"eighteen",18},{"nineteen",19},{"twenty",20},{"thirty",30},{"forty",40},
        {"fifty",50},{"sixty",60},{"seventy",70},{"eighty",80},{"ninety",90},
        {"hundred",100},{"thousand",1000}
    };
    List<long> numbers = Regex.Matches(numberString, @"\w+").Cast<Match>().Select(m => m.Value.ToLowerInvariant()).Where(v => numberTable.ContainsKey(v)).Select(v => numberTable[v]).ToList();
    long remain = 0, total = 0L;
    foreach (var num in numbers)
    {
        if (num >= 1000)
        {
            total += remain * num;
            remain = 0;
        }
        else if (num >= 100) remain *= num;
        else remain += num;
    }
    return total + remain;
}
string DecodeText(string text)
{
    Dictionary<string, string> dict = new Dictionary<string, string>
    {
        {"plus","+"},{"substract","-"},{"multiple","*"},{"division","/" },{"equals","="},{"and",""}
    };
    foreach (KeyValuePair<string, string> kvp in dict)
        text = text.Replace(kvp.Key, kvp.Value);
    List<string> lstWordCombo = text.Split('+', '-', '*', '/', '=', '\n').Where(t => !decimal.TryParse(t.Trim(), out _) && t.Trim().Length > 0).Distinct().OrderByDescending(t => t.Length).ToList();
    foreach (string tWord in lstWordCombo)
    {
        string word = tWord.Trim();
        long number = ConvertToNumbers(word);
        text = Regex.Replace(text, @"\b" + word + @"\b", number.ToString());
    }
    return text;
}
using (StreamReader streamReader = new StreamReader("../../../Large_Input.txt"))
{
    int n = Convert.ToInt32(streamReader.ReadLine());
    string Cases = DecodeText(streamReader.ReadToEnd());
    List<string> List = Cases.Split("\n").Take(n).ToList();
    int Count = 1;
    foreach (string tLine in List.Take(970))
    {
        string line = tLine.Replace(" ", "").Replace("\r", "");
        string[] data = line.Split('=');
        string Exp = data[0];
        string ExpResult = data[1];
        Console.WriteLine("Case " + Count++ + "#: " + (EvaluateExpression(Exp).ToString() == ExpResult.Trim()));
    }
}

double EvaluateExpression(string expression)
{
    expression = expression.Trim().Replace("+", " + ").Replace("-", " - ").Replace("*", " * ").Replace("/", " / ");
    List<string> list = expression.Split(' ').Where(x => x.Trim().Length > 0).ToList();
    while(list.Count != 1)
    {
        string operation = "";
        if (list.Contains("/"))
            operation = "/";
        else if (list.Contains("*"))
            operation = "*";
        else if (list.Contains("-"))
            operation = "-";
        else if (list.Contains("+"))
            operation = "+";
        int index = list.IndexOf(operation);
        string first = list[index - 1];
        string last = list[index + 1];
        switch (operation)
        {
            case "+":
                list[index] = (Convert.ToDouble(first) + Convert.ToDouble(last)).ToString();
                break;
            case "-":
                list[index] = (Convert.ToDouble(first) - Convert.ToDouble(last)).ToString();
                break;
            case "*":
                list[index] = (Convert.ToDouble(first) * Convert.ToDouble(last)).ToString();
                break;
            case "/":
                list[index] = (Convert.ToDouble(first) / Convert.ToDouble(last)).ToString();
                break;
        }
        list.RemoveAt(index - 1);
        list.RemoveAt(index);
    }
    return Convert.ToDouble(list.First());
}