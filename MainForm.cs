namespace GeoFeatureFinder
{
  public class MainForm : Form
  {
    private readonly Button runButton;
    private readonly Button inputFileButton;
    private readonly Button outputFileButton;
    private readonly TextBox inputFilePathTextBox;
    private readonly TextBox outputFilePathTextBox;
    private readonly TextBox latitudeTextBox;
    private readonly TextBox longitudeTextBox;
    private readonly ProgressBar progressBar;

    public MainForm()
    {
      // Set application title
      Text = "Geo Feature Finder";

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

      inputFilePathTextBox = new TextBox
      {
        PlaceholderText = "Enter input file path (Your GeoJSON file)",
        Dock = DockStyle.Top,
      };

      outputFilePathTextBox = new TextBox
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
      panel.Controls.Add(inputFilePathTextBox, 1, 0);
      panel.Controls.Add(inputFileButton, 2, 0);
      panel.Controls.Add(latitudeLabel, 0, 1);
      panel.Controls.Add(latitudeTextBox, 1, 1);
      panel.Controls.Add(longitudeLabel, 0, 2);
      panel.Controls.Add(longitudeTextBox, 1, 2);
      panel.Controls.Add(outputFilePathLabel, 0, 3);
      panel.Controls.Add(outputFilePathTextBox, 1, 3);
      panel.Controls.Add(outputFileButton, 2, 3);
      panel.Controls.Add(runButton, 1, 4);
      panel.Controls.Add(progressBarLabel, 0, 4);
      panel.Controls.Add(progressBar, 1, 4);

      // Set TabIndex
      inputFilePathTextBox.TabIndex = 0;
      inputFileButton.TabIndex = 1;
      latitudeTextBox.TabIndex = 2;
      longitudeTextBox.TabIndex = 3;
      outputFilePathTextBox.TabIndex = 4;
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
          inputFilePathTextBox.Text = openFileDialog.FileName;
        }
      }
    }

    private void OutputFileButton_Click(object? sender, EventArgs e)
    {
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
          outputFilePathTextBox.Text = saveFileDialog.FileName;
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

        string inputPath = inputFilePathTextBox.Text;
        string outputPath = outputFilePathTextBox.Text;

        if (string.IsNullOrWhiteSpace(inputPath))
        {
          MessageBox.Show("Please enter a valid input file path.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
          MessageBox.Show("Please enter a valid output path.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (!double.TryParse(latitudeTextBox.Text, out double latitude))
        {
          MessageBox.Show("Please enter a valid latitude.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (!double.TryParse(longitudeTextBox.Text, out double longitude))
        {
          MessageBox.Show("Please enter a valid longitude.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (latitude < -90 || latitude > 90)
        {
          MessageBox.Show("Latitude must be between -90 and 90.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (longitude < -180 || longitude > 180)
        {
          MessageBox.Show("Longitude must be between -180 and 180.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        Program.RunGeoFeatureFinder(inputPath, outputPath, latitude, longitude);

        progressBar.Value = 100;

        MessageBox.Show($"Output has been successfully written to '{outputPath}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception err)
      {
        MessageBox.Show($"An error occurred: {err.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}