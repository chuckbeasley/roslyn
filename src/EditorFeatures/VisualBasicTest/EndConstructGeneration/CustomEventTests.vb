﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.UnitTests.EndConstructGeneration
    <[UseExportProvider]>
    <Trait(Traits.Feature, Traits.Features.EndConstructGeneration)>
    Public Class CustomEventTests
        <WpfFact>
        Public Async Function TestApplyAfterCustomEvent() As Task
            Await VerifyStatementEndConstructAppliedAsync(
                before:="Class c1
    Custom Event goo As System.EventHandler
End Class",
                beforeCaret:={1, -1},
                after:="Class c1
    Custom Event goo As System.EventHandler
        AddHandler(value As EventHandler)

        End AddHandler
        RemoveHandler(value As EventHandler)

        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)

        End RaiseEvent
    End Event
End Class",
                afterCaret:={3, -1})
        End Function

        <WpfFact>
        Public Async Function TestApplyAfterCustomEventWithImportsStatement() As Task
            Await VerifyStatementEndConstructAppliedAsync(
                before:="Imports System
Class c1
    Custom Event goo As EventHandler
End Class",
                beforeCaret:={2, -1},
                after:="Imports System
Class c1
    Custom Event goo As EventHandler
        AddHandler(value As EventHandler)

        End AddHandler
        RemoveHandler(value As EventHandler)

        End RemoveHandler
        RaiseEvent(sender As Object, e As EventArgs)

        End RaiseEvent
    End Event
End Class",
                afterCaret:={4, -1})
        End Function

        <WpfFact>
        Public Async Function TestApplyAfterCustomEventWithMissingDelegateType() As Task
            Await VerifyStatementEndConstructAppliedAsync(
                before:="Imports System
Class c1
    Custom Event goo As GooHandler
End Class",
                beforeCaret:={2, -1},
                after:="Imports System
Class c1
    Custom Event goo As GooHandler
        AddHandler(value As GooHandler)

        End AddHandler
        RemoveHandler(value As GooHandler)

        End RemoveHandler
        RaiseEvent()

        End RaiseEvent
    End Event
End Class",
                afterCaret:={4, -1})
        End Function

        <WpfFact>
        Public Async Function TestApplyAfterCustomEventWithNonDelegateType() As Task
            Await VerifyStatementEndConstructAppliedAsync(
                before:="Imports System
Class c1
    Custom Event goo As Object
End Class",
                beforeCaret:={2, -1},
                after:="Imports System
Class c1
    Custom Event goo As Object
        AddHandler(value As Object)

        End AddHandler
        RemoveHandler(value As Object)

        End RemoveHandler
        RaiseEvent()

        End RaiseEvent
    End Event
End Class",
                afterCaret:={4, -1})
        End Function

        <WpfFact>
        Public Async Function TestApplyAfterCustomEventWithGenericType() As Task
            Await VerifyStatementEndConstructAppliedAsync(
                before:="Imports System
Class c1
    Custom Event goo As EventHandler(Of ConsoleCancelEventArgs)
End Class",
                beforeCaret:={2, -1},
                after:="Imports System
Class c1
    Custom Event goo As EventHandler(Of ConsoleCancelEventArgs)
        AddHandler(value As EventHandler(Of ConsoleCancelEventArgs))

        End AddHandler
        RemoveHandler(value As EventHandler(Of ConsoleCancelEventArgs))

        End RemoveHandler
        RaiseEvent(sender As Object, e As ConsoleCancelEventArgs)

        End RaiseEvent
    End Event
End Class",
                afterCaret:={4, -1})
        End Function

        <WpfFact>
        Public Async Function DoNotApplyAfterCustomEventAlreadyTerminated() As Task
            Await VerifyStatementEndConstructNotAppliedAsync(
                text:="Imports System
Class c1
    Custom Event goo As EventHandler(Of ConsoleCancelEventArgs)
        AddHandler(value As EventHandler(Of ConsoleCancelEventArgs))

        End AddHandler
        RemoveHandler(value As EventHandler(Of ConsoleCancelEventArgs))

        End RemoveHandler
        RaiseEvent(sender As Object, e As ConsoleCancelEventArgs)

        End RaiseEvent
    End Event
End Class",
                caret:={2, -1})
        End Function
    End Class
End Namespace
