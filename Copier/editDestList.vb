Public Class editDestList
    Dim fd As FolderBrowserDialog = New FolderBrowserDialog
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles OkBtn.Click
        Dim newPath As String = TextBox1.Text
        If newPath = "" Then
            Label2.Visible = True
        Else
            Dim pathWithoutWhitSpaces As String = RemoveWhitespace(newPath)
            Form1.CheckedListBox2.Items.Add(newPath)
            Form1.destList.Add(pathWithoutWhitSpaces)
            Me.Close()
        End If

    End Sub

    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If fd.ShowDialog() = DialogResult.OK Then

            TextBox1.Text = fd.SelectedPath

        End If
    End Sub

    Private Sub editDestList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Visible = False
    End Sub

End Class