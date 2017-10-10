Public Class Form1
    Dim GroundLegs As Integer = 2
    Dim Layers As Integer = 1
    Dim Line(GroundLegs, Layers) As CLegs
    Dim floor As CFloor
    Dim Rnd As New Random
    Dim joint(GroundLegs, Layers) As CJoints
    Dim P1 As Integer = 100
    Dim down As Boolean = False
    Dim Up As Boolean = True
    Dim Body As CBodies
    Dim P1Legs(GroundLegs) As PointF



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'create floor object
        floor = New CFloor(500)
        Body = New CBodies(100, 0, 300, 0)
        'create legs and joints
        For y = 0 To Layers
            If y = 1 Then
                P1 -= (GroundLegs + 1) * 100
            End If
            For x = 0 To GroundLegs
                If y = 1 Then
                    Line(x, y) = New CLegs(P1, 0, Rnd)
                    joint(x, y) = New CJoints(P1, 0, 10, 10, 1)
                    P1Legs(x) = Line(x, y).p1
                Else
                    Line(x, y) = New CLegs(P1, 100, Rnd)
                    joint(x, y) = New CJoints(P1, 100, 10, 10, 1)
                End If
                
                P1 += 100
            Next



        Next
        P1 -= 300
        Body.BodyPoints(P1Legs, GroundLegs)
    End Sub




    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'keep display refreshing, loop through display
        Display.Refresh()
    End Sub

    Private Sub Display_Paint(sender As Object, e As PaintEventArgs) Handles Display.Paint
        Dim g As Graphics
        g = e.Graphics

        'draw the floor
        floor.draw(g)

        For y = 0 To Layers
            For x = 0 To GroundLegs
                Line(x, y).AngleLock(floor)




                If Line(x, y).py2 >= floor.ypos Then

                    Line(x, y).Friction()
                Else
                    Line(x, y).NewPoints()

                End If


            Next
        Next

        For y = 0 To Layers
            For x = 0 To GroundLegs
                If y = 0 Then
                    Line(x, y).px2 += Line(x, y + 1).px2 - Line(x, y).px1
                    Line(x, y).px1 = Line(x, y + 1).px2
                    Line(x, y).py2 += Line(x, y + 1).py2 - Line(x, y).py1
                    Line(x, y).py1 = Line(x, y + 1).py2
                End If
                If y = 0 Then
                    If floor.ypos <= Line(x, y).py1 Or floor.ypos <= Line(x, y).py2 Then
                        Line(x, y).Yspeed = 0

                        Line(x, y + 1).px1 += Line(x, y).px1 - Line(x, y + 1).px2
                        Line(x, y + 1).px2 = Line(x, y).px1
                        Line(x, y + 1).py1 += Line(x, y).py1 - Line(x, y + 1).py2
                        Line(x, y + 1).py2 = Line(x, y).py1
                        Line(x, y + 1).py1 -= (Line(x, y).py2 - floor.ypos)
                        Line(x, y + 1).LYpos -= (Line(x, y).py2 - floor.ypos)
                        Line(x, y + 1).py2 -= (Line(x, y).py2 - floor.ypos)
                    End If

                    If Line(x, y).Yspeed = 0 Then
                        Line(x, y + 1).Yspeed = 0
                    End If
                End If
                Line(x, y).HitFloor(floor)

                If Line(x, y).CheckFloor(floor) = True Then
                    Body.yspeed = 0
                    If Line(x, y).px1 < Body.CoM.X Then
                        Body.Left = True
                    ElseIf Line(x, y).px1 > Body.CoM.X Then
                        Body.Right = True
                    Else
                        Body.Right = True
                        Body.Left = True
                    End If
                End If

                joint(x, y).rise(Line(x, y).p1)
                Line(x, y).draw(g, floor)
                joint(x, y).draw(g)
                Line(x, y).drop()
                joint(x, y).drop(floor)

                'Line(x, y).Yspeed += 1
                'joint(x, y).Yspeed += 1
            Next
        Next

        If Body.Right = False And Body.Left = False Then

        ElseIf Body.Right = False Then

        ElseIf Body.Left = False Then

        End If

        Body.draw(g)

        For x = 0 To GroundLegs
            Line(x, 1).p2.Y += Body.connections(x).Y - Line(x, 1).p1.Y
            Line(x, 1).py2 += Body.connections(x).Y - Line(x, 1).p1.Y
            Line(x, 1).p1 = Body.connections(x)
            Line(x, 1).LYpos = Line(x, 1).p1.Y
            Line(x, 1).py1 = Line(x, 1).p1.Y
        Next

        Body.drop()

    End Sub



    Private Sub Display_Click(sender As Object, e As EventArgs) Handles Display.Click

    End Sub

    'TODO: make joints always spawn one one side of leg, make sure that there is an end to the leg, make it so legs can connect to joint, 2 legs to one joint etc. make end legs have friction to move forwards. make body and centre of mass and if centre of mass is away from pivot that it falls - whole object rotates 



End Class
