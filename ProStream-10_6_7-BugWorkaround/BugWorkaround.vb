Module BugWorkaround

    Sub Main()
        Try
            Dim Filename As String = ""
            Dim args = My.Application.CommandLineArgs
            If args.Count() > 0 Then
                Filename = args.Item(0).ToString
            Else
                Throw New MissingArgumentException("Please run on console with a filename argument, or drag and drop an input file on icon or shortcut.")
            End If


            Dim ReadHandle As IO.StreamReader = FileIO.FileSystem.OpenTextFileReader(Filename)
            'could be a big file...
            Dim FileText As String = ReadHandle.ReadToEnd()
            ReadHandle.Close()

            Dim NumOccurences As Integer = 0
            NumOccurences = FindWords(FileText, "RegenerateTables=""0""")
            Dim Replaced As String = Replace(FileText, "RegenerateTables=""0""", "RegenerateTables=""1""")

            If FindWords(Replaced, "RegenerateTables=""0""") > 0 Then
                Throw New ConstraintException("Currently written to set all occurences")
            End If

            Dim FilenameInfo As System.IO.FileInfo
            FilenameInfo = My.Computer.FileSystem.GetFileInfo(Filename)
            Dim FileFolderPath As String = FilenameInfo.DirectoryName
            Dim ChangedFilename As String = Replace(FilenameInfo.Name, ".xml", "-RegenTables.xml")

            Dim WriteHandle As IO.StreamWriter = FileIO.FileSystem.OpenTextFileWriter(FileFolderPath & "\" & ChangedFilename, False)
            WriteHandle.Write(Replaced)
            WriteHandle.Close()

            Console.Out().WriteLine("Changed " & NumOccurences & " occurences In " & Filename)
        Catch e As MissingArgumentException
            Console.Out().WriteLine(e.Message)
        Finally
            Console.Out().WriteLine("Press Enter to continue...")
            Console.ReadLine()
        End Try

    End Sub

    ' From http://www.visual-basic-tutorials.com/Tutorials/Strings/count-how-many-times-a-string-occurs-in-visual-basic.htm
    Private Function FindWords(ByVal TextSearched As String, ByVal Paragraph As String) As Integer
        Dim location As Integer = 0
        Dim occurances As Integer = 0

        Do
            location = TextSearched.IndexOf(Paragraph, location)
            If location <> -1 Then
                occurances += 1
                location += Paragraph.Length
            End If

        Loop Until location = -1
        Return occurances
    End Function
    Class MissingArgumentException
        Inherits ConstraintException
        Public Sub New(s As String)
            MyBase.New(s)
        End Sub
    End Class

End Module
