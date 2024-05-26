Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class CellSpacingEvent
    Implements IPdfPCellEvent
    Private cellSpacing As Integer
    Public Sub New(cellSpacing As Integer)
        Me.cellSpacing = cellSpacing
    End Sub
    Private Sub CellLayout(cell As PdfPCell, position As Rectangle, canvases As PdfContentByte()) Implements IPdfPCellEvent.CellLayout
        'Grab the line canvas for drawing lines on
        Dim cb As PdfContentByte = canvases(PdfPTable.LINECANVAS)
        'Create a new rectangle using our previously supplied spacing
        cb.Rectangle(position.Left + Me.cellSpacing, position.Bottom + Me.cellSpacing, (position.Right - Me.cellSpacing) - (position.Left + Me.cellSpacing), (position.Top - Me.cellSpacing) - (position.Bottom + Me.cellSpacing))
        'Set a color
        cb.SetColorStroke(iTextSharp.text.Color.RED)
        'Draw the rectangle
        cb.Stroke()
    End Sub
End Class
