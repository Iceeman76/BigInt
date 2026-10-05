Add-Type -AssemblyName System.Windows.Forms.DataVisualization
Add-Type -AssemblyName System.Drawing

$measurements = Import-Csv (Join-Path $PSScriptRoot 'performance.csv')
$chart = New-Object System.Windows.Forms.DataVisualization.Charting.Chart
$chart.Width = 1400
$chart.Height = 850
$chart.BackColor = [System.Drawing.Color]::White
$chart.AntiAliasing = 'All'
$chart.TextAntiAliasingQuality = 'High'

$title = $chart.Titles.Add('Pi series iteration count')
$title.Font = New-Object System.Drawing.Font('Segoe UI', 23, [System.Drawing.FontStyle]::Bold)
$title.ForeColor = [System.Drawing.ColorTranslator]::FromHtml('#172033')
$subtitle = $chart.Titles.Add('20 guard digits | Machin includes both arctan series | fewer iterations is better')
$subtitle.Font = New-Object System.Drawing.Font('Segoe UI', 13)
$subtitle.ForeColor = [System.Drawing.ColorTranslator]::FromHtml('#526079')

$area = New-Object System.Windows.Forms.DataVisualization.Charting.ChartArea
$area.AxisX.Title = 'Decimal places'
$area.AxisY.Title = 'Loop iterations'
$area.AxisX.Minimum = 0
$area.AxisX.Maximum = 105000
$area.AxisX.Interval = 25000
$area.AxisX.LabelStyle.Format = '#,##0'
$area.AxisY.LabelStyle.Format = '#,##0'
$area.AxisY.Minimum = 0
$area.AxisY.Maximum = 180000
$area.AxisY.Interval = 30000
foreach ($axis in @($area.AxisX, $area.AxisY)) {
    $axis.TitleFont = New-Object System.Drawing.Font('Segoe UI', 15)
    $axis.LabelStyle.Font = New-Object System.Drawing.Font('Segoe UI', 12)
    $axis.LineColor = [System.Drawing.ColorTranslator]::FromHtml('#8c98aa')
    $axis.MajorGrid.LineColor = [System.Drawing.ColorTranslator]::FromHtml('#e3e8ef')
    $axis.MajorTickMark.LineColor = [System.Drawing.ColorTranslator]::FromHtml('#8c98aa')
}
$chart.ChartAreas.Add($area)

$legend = New-Object System.Windows.Forms.DataVisualization.Charting.Legend
$legend.Docking = 'Bottom'
$legend.Alignment = 'Center'
$legend.Font = New-Object System.Drawing.Font('Segoe UI', 14)
$chart.Legends.Add($legend)

foreach ($algorithm in @('Arcsin', 'Machin')) {
    $series = New-Object System.Windows.Forms.DataVisualization.Charting.Series($algorithm)
    $series.ChartType = 'Line'
    $series.BorderWidth = 4
    $series.MarkerStyle = 'Circle'
    $series.MarkerSize = 11
    $series.Color = [System.Drawing.ColorTranslator]::FromHtml($(if ($algorithm -eq 'Arcsin') { '#2563eb' } else { '#d97706' }))
    foreach ($measurement in $measurements) {
        $iterations = if ($algorithm -eq 'Arcsin') { [int]$measurement.arcsin_iterations } else { [int]$measurement.machin_total_iterations }
        $null = $series.Points.AddXY([int]$measurement.digits, $iterations)
    }
    $chart.Series.Add($series)
}

$chart.SaveImage((Join-Path $PSScriptRoot 'performance.png'), [System.Windows.Forms.DataVisualization.Charting.ChartImageFormat]::Png)
$chart.Dispose()
$measurements | Format-Table

