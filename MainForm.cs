namespace GeoFeatureFinder
{
  public class MainForm : Form
  {
    private Button runButton;
    private Button inputFileButton;
    private Button outputFileButton;
    private TextBox filePathTextBox;
    private TextBox outputPathTextBox;
    private TextBox latitudeTextBox;
    private TextBox longitudeTextBox;

    public MainForm()
    {
      runButton = new Button
      {
        Text = "Run",
        Dock = DockStyle.Top,
      };
      runButton.Click += RunButton_Click;

      inputFileButton = new Button
      {
        Text = "...",
        Dock = DockStyle.Top,
      };
      inputFileButton.Click += InputFileButton_Click;

      outputFileButton = new Button
      {
        Text = "...",
        Dock = DockStyle.Top,
      };
      outputFileButton.Click += OutputFileButton_Click;

      filePathTextBox = new TextBox
      {
        PlaceholderText = "Enter input file path (Your GeoJSON file)",
        Dock = DockStyle.Top,
      };

      outputPathTextBox = new TextBox
      {
        PlaceholderText = "Enter output path",
        Dock = DockStyle.Top,
      };

      latitudeTextBox = new TextBox
      {
        PlaceholderText = "Enter latitude",
        Dock = DockStyle.Top,
      };

      longitudeTextBox = new TextBox
      {
        PlaceholderText = "Enter longitude",
        Dock = DockStyle.Top,
      };

      Controls.Add(runButton);
      Controls.Add(outputFileButton);
      Controls.Add(outputPathTextBox);
      Controls.Add(longitudeTextBox);
      Controls.Add(latitudeTextBox);
      Controls.Add(inputFileButton);
      Controls.Add(filePathTextBox);
    }

    private void InputFileButton_Click(object? sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          filePathTextBox.Text = openFileDialog.FileName;
        }
      }
    }

    private void OutputFileButton_Click(object? sender, EventArgs e)
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
          outputPathTextBox.Text = saveFileDialog.FileName;
        }
      }
    }

    private void RunButton_Click(object? sender, EventArgs e)
    {
      string filePath = filePathTextBox.Text;
      string outputPath = outputPathTextBox.Text;
      double latitude = double.Parse(latitudeTextBox.Text);
      double longitude = double.Parse(longitudeTextBox.Text);

      Program.RunGeoFeatureFinder(filePath, outputPath, latitude, longitude);
    }
  }
}