Option Explicit On
Option Strict On
Option Infer Off
' Project name:         Trivia Project
' Project purpose:      Displays trivia questions and
'                       answers and the number of incorrect
'                       answers made by the user
' Created/revised by:   Nicholas Scifert on 09/26/2020

Imports System.ComponentModel.Design
Imports System.Text

Public Class MainForm


    Private results(8) As String                                        'Holds a string that lets the program know the the problem is right or wrong

    'Function used to activate and deactivate the text boxes and buttons 
    'to allow the user to know that there answer has been submited
    Private Sub IsActive(ByVal var As String)

        Dim x As String

        x = var

        If results(TblGameBindingSource.Position) = String.Empty Then
            'if answer has not been submited (Activate text boxes and buttons)
            questionTextBox.Enabled = True
            aTextBox.Enabled = True
            bTextBox.Enabled = True
            cTextBox.Enabled = True
            dTextBox.Enabled = True
            aRadioButton.Enabled = True
            bRadioButton.Enabled = True
            cRadioButton.Enabled = True
            dRadioButton.Enabled = True

            changeButton.Enabled = False                'Stops the user from using the change button
            submitButton.Enabled = True                 'Allows the user to use the submit button

        Else
            'if answer has been submited  (Deactivate text boxes and buttons)
            questionTextBox.Enabled = False
            aTextBox.Enabled = False
            bTextBox.Enabled = False
            cTextBox.Enabled = False
            dTextBox.Enabled = False
            aRadioButton.Enabled = False
            bRadioButton.Enabled = False
            cRadioButton.Enabled = False
            dRadioButton.Enabled = False

            changeButton.Enabled = True                 'Allows the user to use the change button
            submitButton.Enabled = False                'Stops the user from using the submit button
            incorrectButton.Enabled = True              'Allows the user to use the number incorrect button

        End If

    End Sub

    'exits the program
    Private Sub exitButton_Click(sender As Object, e As EventArgs) Handles exitButton.Click
        Me.Close()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'TriviaDataSet.tblGame' table. You can move, or remove it, as needed.
        Me.TblGameTableAdapter.Fill(Me.TriviaDataSet.tblGame)

        'Displays the quesiton numbers
        questionNumLabel.Text = "Question " + (TblGameBindingSource.Position + 1).ToString + ":"


    End Sub

    Private Sub newButton_Click(sender As Object, e As EventArgs) Handles newButton.Click
        ' starts a new game
        TblGameBindingSource.MoveFirst()
        questionNumLabel.Text = "Question " + (TblGameBindingSource.Position + 1).ToString + ":"

        'resets the results Array
        For i As Integer = 0 To 8
            results(i) = String.Empty
        Next i



        Dim answerPosition As Integer                   'puts a value of 0 in questionAnswered Array to indicated the question


        nextButton.Enabled = True                       'lets the next button be used by the user
        previousButton.Enabled = False                  'the previous button will not beable to be used by the user

        Call IsActive(results(answerPosition))     'Function Call to the IsActive Function 

    End Sub

    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        ' determines whether the user's answer is correct
        ' and the number of incorrect answers

        Dim answerPosition As Integer

        answerPosition = TblGameBindingSource.Position          'Stores the position of the quesiton 

        Dim userAnswer As String                                'Stores which answer the user submited
        Dim ptrPosition As Integer

        ' store record pointer's position
        ptrPosition = TblGameBindingSource.Position             'Assigns the current record's position to the ptrPosition variable


        ' determine selected radio button
        Select Case True
            Case aRadioButton.Checked
                userAnswer = aRadioButton.Text.Substring(1, 1)
            Case bRadioButton.Checked
                userAnswer = bRadioButton.Text.Substring(1, 1)
            Case cRadioButton.Checked
                userAnswer = cRadioButton.Text.Substring(1, 1)
            Case Else
                userAnswer = dRadioButton.Text.Substring(1, 1)
        End Select

        ' updates the number of incorrect answers
        If userAnswer <>
                TriviaDataSet.tblGame(ptrPosition).CorrectAnswer Then
            results(ptrPosition) = "W"                     'Answer has been answwered wrong
        Else
            results(ptrPosition) = "C"                     'Answer has been answered correct
        End If



        Call IsActive(results(answerPosition))         'Function Call to the IsActive Function

        newButton.Enabled = True                            'Allows the user to use the next button


    End Sub


    Private Sub nextButton_Click(sender As Object, e As EventArgs) Handles nextButton.Click
        TblGameBindingSource.MoveNext()                     'moves the record pointer to the next record in the dataset

        questionNumLabel.Text = "Question " + (TblGameBindingSource.Position + 1).ToString + ":"

        Dim answerPosition As Integer

        answerPosition = TblGameBindingSource.Position      'Stores the quesitons postion in the dataset

        Call IsActive(results(answerPosition))         'Calls the IsActive Functiion 

        incorrectButton.Enabled = False                     'Stops the user from using the number incorrect button
        previousButton.Enabled = True                       'Allows the user to use the previous button

        If TblGameBindingSource.Position + 1 >= 9 Then      'Checks to see if the record pointer is equal to the amount of quesitons in the dataset
            nextButton.Enabled = False                      'If they are equal, the user can not use the next button at this time
        End If




    End Sub

    Private Sub previousButton_Click(sender As Object, e As EventArgs) Handles previousButton.Click
        TblGameBindingSource.MovePrevious()                 'Moves the record pointer to the previous record in the dataset

        questionNumLabel.Text = "Question " + (TblGameBindingSource.Position + 1).ToString + ":"

        Dim answerPosition As Integer

        answerPosition = TblGameBindingSource.Position      'Stores the quesitons postion in the dataset

        Call IsActive(results(answerPosition))         'Calls the IsActive Functiion 

        incorrectButton.Enabled = False                     'Stops the user from using the incorrect button
        nextButton.Enabled = True                           'Allows the user to use the next button

    End Sub

    Private Sub incorrectButton_Click(sender As Object, e As EventArgs) Handles incorrectButton.Click

        Dim answered As Integer
        Dim incorrect As Integer
        incorrect = 0                       'Assigns zero to the variable incorrect

        For i As Integer = 0 To 8

            If results(i) <> String.Empty Then          'Only counts the queitons answered once
                answered += 1
            End If

            If results(i) = "W" Then                    'Counts how many questions are wrong
                incorrect += 1
            End If
        Next


        MessageBox.Show("Number Incorrect: " & incorrect.ToString & " incorrect out of " & answered.ToString, 'A message box to show how many the user has gotten incorrect
                        "Number Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Information)                          'compared to the number of quesitons the user has answered

    End Sub

    Private Sub changeButton_Click(sender As Object, e As EventArgs) Handles changeButton.Click
        questionNumLabel.Text = "Question " + (TblGameBindingSource.Position + 1).ToString + ":"

        Dim answerPosition As Integer
        answerPosition = TblGameBindingSource.Position      'Stores the quesitons postion in the dataset

        results(answerPosition) = String.Empty


        Call IsActive(results(answerPosition))         'Calls the IsActive Function 



    End Sub
End Class
