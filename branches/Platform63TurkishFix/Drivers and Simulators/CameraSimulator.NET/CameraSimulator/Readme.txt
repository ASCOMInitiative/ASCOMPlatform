Version 2 .NET Camera simulator - Beta

Install in the usual way.

The Setup dialog specifies the camera properties in the usual way.

The CCD temperature regulation will adjust the power to keep the temperature close to that specified.

The simulation is generated as follows:

An image can be specified. I've only tried a jpg image but any normal image type can beloaded. It expects a colour image.
The image is loaded into an image data array with the camera type and bayer offsets applied:
For monochrome the brightness is used.
For RGGB, CYMG, CMYG2 and LRGB the appropriate colours and bayer offsets are used.
This hasn't been fully tested.
All image values are scaled to 0 to 255.

At the end of an exposure the image array is extracted from the image data using the start, num and bin values, and multiplied by the exposure time in seconds.

If noise is specified then an offset of 3 plus dark current calculated from the exposure time and ccd temperature, assuming 1 ADU at 0 deg C and halving or doubling for every 5 deg difference is added and the value this gives used to get a poisson distributed value, for values over 50 a normal distribution is used.
The result is clipped to the Max ADU value and put into the image array.

If the shutter exists and is closed then the image data is not added but the noise and dark current is.

It passes the current conform, except for an error that's something to do with Conform.

The V2 properties are only available late bound, I'm not sure about the others, I had to change the the ClassInterfaceType from None to AutoDispatch to get the late bound properties.

I'd be interested to hear if this is useful.

Chris Rowland






