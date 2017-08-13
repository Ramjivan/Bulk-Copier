Imports System
Imports System.IO
Imports System.Collections

Public Structure cList
    Public ID As Integer
    Public Name As String
    Public Path As String
    Public isChecked As Boolean
End Structure

Public Class Form1

    'The messages to look for.
    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Private Const DBT_DEVTYP_VOLUME As Integer = &H2  '
    '
    'Get the information about the detected volume.
    Private Structure DEV_BROADCAST_VOLUME

        Dim Dbcv_Size As Integer

        Dim Dbcv_Devicetype As Integer

        Dim Dbcv_Reserved As Integer

        Dim Dbcv_Unitmask As Integer

        Dim Dbcv_Flags As Short

    End Structure


    'Declaring Variables
    Dim fd As FolderBrowserDialog = New FolderBrowserDialog
    Dim source_Path As String = ""
    Dim dest_path As String
    Public destList As New List(Of String)
    Public sourceList As New List(Of cList)
    Public source As cList
    Dim count As Integer = 0
    Dim xcpStr As String = ""




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Start_btn.Click

        For Each dest_path As String In destList
            If Directory.Exists(dest_path) Then

                formateBeforeCopy(dest_path)

                'Counter Updation
                count += 1
                Label18.Text = count
                'source paths
                For index = 0 To sourceList.Count - 1

                    source = sourceList(index)

                    source.isChecked = False

                    sourceList(index) = source

                Next

                For Each indexChecked As Integer In CheckedListBox1.CheckedIndices

                    For index = 0 To sourceList.Count - 1
                        source = sourceList(index)

                        If source.ID = indexChecked Then
                            source.isChecked = True

                            sourceList(index) = source
                        End If
                    Next
                Next

                'copy process
                For index = 0 To sourceList.Count - 1
                    source = sourceList(index)
                    If source.isChecked = True Then
                        xCopy(source.Path, dest_path)
                    End If
                Next

            Else
                Continue For
            End If


        Next
        ProgressBar1.Value = 100
        ProgressBar1.MarqueeAnimationSpeed = 3000
    End Sub
    Public Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function

    Public Sub xCopy(ByVal source_Path As String, ByVal dest_path As String)
        Dim copyMode As String
        If cliRadioButton.Checked And cliModeComboBox.SelectedIndex = 0 Then

            copyMode = "robocopy"
        Else
            copyMode = "xcopy"
        End If
        xcpStr = ""
        xcpStr += source_Path               'Making string for
        xcpStr += " "                       'Xcopy   
        xcpStr += dest_path
        xcpStr += " /s /r:1"

        Dim pro As Process = New Process

        pro.StartInfo.FileName = copyMode
        pro.StartInfo.Arguments = xcpStr
        pro.Start()

    End Sub

    Public Sub formateBeforeCopy(ByVal destinationPath As String)

        If CheckBox1.Checked Then

            Try
                For Each deleteFile In Directory.GetFiles(destinationPath, "*.*", SearchOption.AllDirectories)

                    File.Delete(deleteFile)
                Next

                For Each deleteFolder In Directory.GetDirectories(destinationPath)
                    System.IO.Directory.Delete(deleteFolder, True)
                Next
            Catch ex As Exception

            End Try



        End If

    End Sub


    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click

        ProgressBar1.Value = 0
        count = 0
        Label18.Text = count

        For item As Integer = 1 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemCheckState(item, CheckState.Unchecked)
        Next
        For index = 0 To sourceList.Count - 1
            source = sourceList(index)
            source.isChecked = False
            sourceList.Add(source)
        Next
    End Sub




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        scanDrives()

        CheckedListBox1.CheckOnClick = False
        cliRadioButton.PerformClick()

        If cliModeComboBox.Items.Count > 0 Then
            cliModeComboBox.SelectedIndex = 0    ' The first item has index 0 '
        End If

        Dim textFilePath As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Bulk_Copier\sourceList.txt"
        If Not File.Exists(textFilePath) Then
            System.IO.Directory.CreateDirectory(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Bulk_Copier")
            File.Create(textFilePath)
        Else
            Dim reader = File.OpenText(textFilePath)
            Dim line As String = Nothing
            Dim lines As Integer = 0

            Dim srSource As cList
            While (reader.Peek() <> -1)
                line = reader.ReadLine()

                If line.StartsWith("/id""") Then

                    Dim x As String = line
                    x = x.Substring(4, x.IndexOf("""/id") - 4)
                    srSource.ID = Convert.ToInt32(x)
                    lines = lines + 1
                End If

                If line.StartsWith("/n""") Then
                    srSource.Name = line.Substring(3, line.IndexOf("""/n") - 3)
                    lines = lines + 1
                End If

                If line.StartsWith("/p""") Then
                    srSource.Path = line.Substring(3, line.IndexOf("""/p") - 3)
                    lines = lines + 1
                End If

                If line.StartsWith("/b") Then
                    srSource.isChecked = False
                    sourceList.Add(srSource)
                    CheckedListBox1.Items.Add(srSource.Name)
                    lines = lines + 1
                End If

            End While

        End If



        'source.ID = 0
        'source.Name = "Ramprasad Ji Maharaj Audio"
        'source.Path = "D:\RamprasadjiAudio"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)

        'source.ID = 1
        'source.Name = "Ramprasad Ji Maharaj Video"
        'source.Path = "D:\RamprasadJiMaharajVideo"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)

        'source.ID = 2
        'source.Name = "Mohandas Ji Maharaj Audio"
        'source.Path = "D:\MohandasjiMaharaj"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)

        'source.ID = 3
        'source.Name = "Mohandas Ji Maharaj Video"
        'source.Path = "D:\MohanDasJiMaharajVideo"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)

        'source.ID = 4
        'source.Name = "Amritramji Audio"
        'source.Path = "D:\Amritramji"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)

        'source.ID = 5
        'source.Name = "Abhayramji Audio"
        'source.Path = "D:\Abhayramji"
        'source.isChecked = False
        'sourceList.Add(source)
        'CheckedListBox1.Items.Add(source.Name)
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Refresh_btn.Click

        scanDrives()

    End Sub

    Public Sub scanDrives()
        For i As Integer = CheckedListBox2.Items.Count - 1 To 0 Step -1

            CheckedListBox2.Items.RemoveAt(i)

        Next
        For Each Drive In My.Computer.FileSystem.Drives
            'Gets drive letter and type
            Dim DriveInfo As String = Drive.Name & " (" & Drive.DriveType.ToString & ")"

            'Checks to see if drive is a removable drive
            Dim removable = "Removable"
            Dim isFlashDrive As Boolean = DriveInfo.Contains(removable)

            'Adds only removable drives to the list
            If isFlashDrive And Not CheckedListBox2.Items.Contains(DriveInfo) Then
                CheckedListBox2.Items.Add(DriveInfo, True)

                destList.Add(Drive.Name)
            End If
        Next



    End Sub


    Protected Overrides Sub WndProc(ByRef M As System.Windows.Forms.Message)
        '
        'These are the required subclassing codes for detecting device based removal and arrival.
        '
        If M.Msg = WM_DEVICECHANGE Then

            Select Case M.WParam
            '
            'Check if a device was added.
                Case DBT_DEVICEARRIVAL

                    Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)

                    If DevType = DBT_DEVTYP_VOLUME Then

                        Dim Vol As New DEV_BROADCAST_VOLUME

                        Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))

                        If Vol.Dbcv_Flags = 0 Then

                            For i As Integer = 0 To 20

                                If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                    Dim Usb As String = Chr(65 + i) + ":\"


                                    If Not CheckedListBox2.Items.Contains(Usb.ToString & " (Removable)") Then
                                        CheckedListBox2.Items.Add(Usb.ToString & " (Removable)", True)
                                    End If

                                    If Not destList.Contains(Usb.ToString & " (Removable)") Then
                                        destList.Add(Usb.ToString)
                                    End If


                                    Exit For

                                End If

                            Next

                        End If

                    End If
                '
                'Check if the message was for the removal of a device.
                Case DBT_DEVICEREMOVECOMPLETE

                    Dim DevType As Integer = Runtime.InteropServices.Marshal.ReadInt32(M.LParam, 4)

                    If DevType = DBT_DEVTYP_VOLUME Then

                        Dim Vol As New DEV_BROADCAST_VOLUME

                        Vol = Runtime.InteropServices.Marshal.PtrToStructure(M.LParam, GetType(DEV_BROADCAST_VOLUME))

                        If Vol.Dbcv_Flags = 0 Then

                            For i As Integer = 0 To 20

                                If Math.Pow(2, i) = Vol.Dbcv_Unitmask Then

                                    Dim Usb As String = Chr(65 + i) + ":\"

                                    If CheckedListBox2.Items.Contains(Usb.ToString & " (Removable)") Then
                                        CheckedListBox2.Items.Remove(Usb.ToString & " (Removable)")
                                    End If

                                    destList.Remove(Usb.ToString)

                                    Exit For

                                End If

                            Next

                        End If

                    End If

            End Select

        End If

        MyBase.WndProc(M)

    End Sub

    Private Sub DeveloperToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()

    End Sub

    Private Sub ScanDrivesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanDrivesToolStripMenuItem.Click

        scanDrives()
    End Sub




    Private Sub AddCustomDestinationPathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddCustomDestinationPathToolStripMenuItem.Click
        editDestList.Show()
    End Sub


    Private Sub SourceListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SourceListToolStripMenuItem.Click
        customSource.Show()
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        'If CheckedListBox1.GetItemCheckState(CheckedListBox1.Items.IndexOf(CheckedListBox1.SelectedItem)) = CheckState.Checked Then
        '    For index = 0 To sourceList.Count - 1
        '        source = sourceList(index)
        '        If source.Name = CheckedListBox1.SelectedItem Then
        '            source.isChecked = True
        '        End If
        '    Next
        'End If

        'source list updation
        'ischecked = false for every element
        'For index = 0 To sourceList.Count - 1
        '    source = sourceList(index)
        '    source.isChecked = False
        '    sourceList.Add(source)
        'Next
        'For Each item As String In CheckedListBox1.CheckedItems
        '    For index = 0 To sourceList.Count - 1
        '        MsgBox(index)
        '        'source = sourceList(index)
        '        'If item = source.Name Then
        '        '    source.isChecked = True
        '        '    sourceList.Add(source)
        '        'End If
        '    Next
        'Next
    End Sub


    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        customSource.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()

    End Sub

    Private Sub AddInitialSourceListTextFileToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub AddSourceListTextFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddSourceListTextFileToolStripMenuItem.Click
        Dim fb As OpenFileDialog = New OpenFileDialog
        Dim strFileName As String

        fb.Title = "Select Initial Source List Text file"
        fb.InitialDirectory = "C:\"
        fb.RestoreDirectory = True

        If fb.ShowDialog() = DialogResult.OK Then
            strFileName = fb.FileName
        End If

    End Sub
End Class
