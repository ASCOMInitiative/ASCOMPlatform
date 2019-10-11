#Region "Enums"
''' <summary>
''' List of available conversion units for use in the Util.ConvertUnits method
''' </summary>
Public Enum Units As Integer
    ' Speed
    metresPerSecond = 0
    milesPerHour = 1
    knots = 2
    'Temperature
    degreesCelsius = 10
    <ObsoleteAttribute("Units.degreesFarenheit is an incorrect spelling and has been deprecated, please use Units.degreesFahrenheit instead.", True)>
    degreesFarenheit = 11
    degreesFahrenheit = 11
    degreesKelvin = 12
    ' Pressure
    hPa = 20
    mBar = 21
    mmHg = 22
    inHg = 23
    'RainRate
    mmPerHour = 30
    inPerHour = 31
End Enum
#End Region