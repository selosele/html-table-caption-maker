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
    /// 오류 메시지1
    /// </summary>
    private readonly static string ERROR_MESSAGE1 = "올바른 table 구조가 아닙니다. 구조를 다시 확인하세요.";

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
        MessageBox.Show(message, "알림");
    }

    /// <summary>
    /// 붙여넣기 버튼을 클릭한다.
    /// </summary>
    private void PasteButton_Click(object sender, RoutedEventArgs e)
    {
        var html = Clipboard.GetText();
        inputTextBox.Text = html;
    }

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
        var html = inputTextBox.Text;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var table = doc.DocumentNode.SelectSingleNode("//table");
        if (table == null) return ERROR_MESSAGE1;

        var thead = table.SelectSingleNode("./thead");
        if (thead == null) return ERROR_MESSAGE1;

        var tr = thead.SelectNodes("./tr");
        if (tr == null) return ERROR_MESSAGE1;

        List<string> thTextList = [];
        for (int i = 0; i < tr.Count; i++)
        {
            var th = tr[i].SelectNodes("./th");
            if (th == null) return ERROR_MESSAGE1;

            for (int j = 0; j < th.Count; j++)
            {
                thTextList.Add(th[j].InnerText.Trim());
            }
        }

        string? outputText;
        if (string.IsNullOrEmpty(tableTitleTextBox.Text))
        {
            outputText = $"<caption>{string.Join(", ", thTextList)}</caption>";
        }
        else
        {
            outputText = $"<caption>{tableTitleTextBox.Text} - {string.Join(", ", thTextList)}</caption>";
        }

        return outputText;
    }

    /// <summary>
    /// 캡션 복사 버튼을 클릭한다.
    /// </summary>
    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(outputTextBox.Text);
    }
}