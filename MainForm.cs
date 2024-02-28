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
    private ProgressBar progressBar;

    public MainForm()
    {
      TableLayoutPanel panel = new TableLayoutPanel
      {
        ColumnCount = 3,
        RowCount = 5,
        Dock = DockStyle.Fill,
      };
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      for (int i = 0; i < panel.RowCount; i++)
      {
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / panel.RowCount));
      }

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

      progressBar = new ProgressBar
      {
        Minimum = 0,
        Maximum = 100,
        Value = 0,
        Dock = DockStyle.Top,
      };

      Label inputFilePathLabel = new Label
      {
        Text = "Input file path:",
        Dock = DockStyle.Top,
      };

      Label latitudeLabel = new Label
      {
        Text = "Latitude:",
        Dock = DockStyle.Top,
      };

      Label longitudeLabel = new Label
      {
        Text = "Longitude:",
        Dock = DockStyle.Top,
      };

      Label outputFilePathLabel = new Label
      {
        Text = "Output file path:",
        Dock = DockStyle.Top,
      };

      Label progressBarLabel = new Label
      {
        Text = "Progress:",
        Dock = DockStyle.Top,
      };

      panel.Controls.Add(inputFilePathLabel, 0, 0);
      panel.Controls.Add(filePathTextBox, 1, 0);
      panel.Controls.Add(inputFileButton, 2, 0);
      panel.Controls.Add(latitudeLabel, 0, 1);
      panel.Controls.Add(latitudeTextBox, 1, 1);
      panel.Controls.Add(longitudeLabel, 0, 2);
      panel.Controls.Add(longitudeTextBox, 1, 2);
      panel.Controls.Add(outputFilePathLabel, 0, 3);
      panel.Controls.Add(outputPathTextBox, 1, 3);
      panel.Controls.Add(outputFileButton, 2, 3);
      panel.Controls.Add(runButton, 1, 4);
      panel.Controls.Add(progressBarLabel, 0, 4);
      panel.Controls.Add(progressBar, 1, 4);

      // Set TabIndex
      filePathTextBox.TabIndex = 0;
      inputFileButton.TabIndex = 1;
      latitudeTextBox.TabIndex = 2;
      longitudeTextBox.TabIndex = 3;
      outputPathTextBox.TabIndex = 4;
      outputFileButton.TabIndex = 5;
      runButton.TabIndex = 6;

      Controls.Add(panel);
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
      try
      {
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = 0;

        string filePath = filePathTextBox.Text;
        string outputPath = outputPathTextBox.Text;
        double latitude = double.Parse(latitudeTextBox.Text);
        double longitude = double.Parse(longitudeTextBox.Text);

        Program.RunGeoFeatureFinder(filePath, outputPath, latitude, longitude);

        progressBar.Value = 100;

        MessageBox.Show("Operation completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception err)
      {
        MessageBox.Show($"An error occurred: {err.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}