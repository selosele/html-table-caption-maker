using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace HtmlTableCaptionMaker;

/// <summary>
/// 메인 윈도우(MainWindow.xaml) 클래스
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 프로그램 종료 메뉴아이템을 클릭한다.
    /// </summary>
    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    /// <summary>
    /// "이 애플리케이션에 대해서" 메뉴아이템을 클릭한다.
    /// </summary>
    private void AboutAppMenuItem_Click(object sender, RoutedEventArgs e)
    {
        var message = string.Join(Environment.NewLine,
            "이 애플리케이션은 .NET(WPF)로 개발되었습니다.",
            "Copyright 2025 selosele.",
            "버전 0.0.1"
        );
        MessageBox.Show(message, "정보");
    }

    /// <summary>
    /// 붙여넣기 버튼을 클릭한다.
    /// </summary>
    private void PasteButton_Click(object sender, RoutedEventArgs e)
        => inputTextBox.Text = Clipboard.GetText();

    /// <summary>
    /// 캡션생성 버튼을 클릭한다.
    /// </summary>
    private void MakeCaptionButton_Click(object sender, RoutedEventArgs e)
        => outputTextBox.Text = MakeCaption();

    /// <summary>
    /// 캡션을 생성해서 반환한다.
    /// </summary>
    private string MakeCaption()
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(inputTextBox.Text);

        var table = doc.DocumentNode.SelectSingleNode("//table");
        if (table == null) return "<table> 태그 누락됨";

        var thead = table.SelectSingleNode("./thead");
        if (thead == null) return "<thead> 태그 누락됨";

        var tr = thead.SelectNodes("./tr");
        if (tr == null) return "<tr> 태그 누락됨";

        List<string> thTextList = [];
        Dictionary<int, HtmlNode> thDict = [];

        var thIndex = 0;
        for (int i = 0; i < tr.Count; i++)
        {
            var th = tr[i].SelectNodes("./th");
            if (th == null) return "<th> 태그 누락됨";

            for (int j = 0; j < th.Count; j++)
            {
                thDict[thIndex] = th[j];
                thIndex++;
            }
        }

        foreach (var x in thDict)
        {
            var th = x.Value;
            var index = x.Key;

            var text = th.InnerText.Trim();
            if (!string.IsNullOrWhiteSpace(text))
            {
                thTextList.Add(text);
            }

            var rowspan = int.Parse(th.Attributes["rowspan"]?.Value ?? "1");
            var colspan = int.Parse(th.Attributes["colspan"]?.Value ?? "1");

            Console.WriteLine($"index: {index}, text: {text}, rowspan: {rowspan}, colspan: {colspan}");
        }

        if (string.IsNullOrWhiteSpace(tableTitleTextBox.Text))
        {
            return $"<caption>{string.Join(", ", thTextList)}</caption>";
        }
        else
        {
            return $"<caption>{tableTitleTextBox.Text} - {string.Join(", ", thTextList)}</caption>";
        }
    }

    /// <summary>
    /// 캡션 복사 버튼을 클릭한다.
    /// </summary>
    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(outputTextBox.Text);
    }
}