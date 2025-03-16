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
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 캡션생성 버튼을 클릭한다.
    /// </summary>
    private void MakeCaptionButton_Click(object sender, RoutedEventArgs e)
    {
        var html = inputTextBox.Text;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var table = doc.DocumentNode.SelectSingleNode("//table");
        var thead = table.SelectSingleNode("./thead");
        var tr = thead.SelectNodes("./tr");
        List<string> thTextList = [];
        for (int i = 0; i < tr.Count; i++)
        {
            var th = tr[i].SelectNodes("./th");
            for (int j = 0; j < th.Count; j++)
            {
                thTextList.Add(th[j].InnerText.Trim());
            }
        }

        var outputText = "";
        if (string.IsNullOrEmpty(tableTitleTextBox.Text))
        {
            outputText = $"<caption>{string.Join(", ", thTextList)}</caption>";
        }
        else
        {
            outputText = $"<caption>{tableTitleTextBox.Text} - {string.Join(", ", thTextList)}</caption>";
        }

        outputTextBox.Text = outputText;
    }
}