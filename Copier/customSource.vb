Public Class customSource
    Dim initialID = Form1.CheckedListBox1.Items.Count
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub CancleBtn_Click(sender As Object, e As EventArgs) Handles CancleBtn.Click
        Me.Close()
    End Sub

    Private Sub OkBtn_Click(sender As Object, e As EventArgs) Handles OkBtn.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            Label3.Visible = True
        Else
            Form1.source.ID = initialID
            initialID += 1
            Form1.source.Name = TextBox1.Text
            Form1.source.Path = TextBox2.Text
            If CheckBox1.Checked Then
                Form1.source.isChecked = True
            Else
                Form1.source.isChecked = False
            End If

            Form1.sourceList.Add(Form1.source)

            Form1.CheckedListBox1.Items.Add(TextBox1.Text, Form1.source.isChecked)
            Me.Close()
        End If
    End Sub

    Private Sub customSource_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Visible = False
    End Sub

    Private Sub BrowseBtn_Click(sender As Object, e As EventArgs) Handles BrowseBtn.Click
        Dim fd As New FolderBrowserDialog
        If fd.ShowDialog() = DialogResult.OK Then
            TextBox2.Text = fd.SelectedPath
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)

    End Sub
End Class