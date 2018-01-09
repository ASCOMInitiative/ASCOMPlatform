//
// This stuff supports the animated collapsible detail text. Note that the dynamic style
// display: xxx must be inline!
//
var timerlen = 10;									// Timing constants
var speed = 2;
var timerID = new Array();							// State variables
var startTime = new Array();						// Associative arrays permit multiple animations
var obj = new Array();
var tobj = new Array();
var curHeight = new Array();
var moving = new Array();
var dir = new Array();

function slidetick(objname) {
	if(dir[objname] == "up") {						// Closing up, stop at 1px
		if(curHeight[objname] > 1) {
			curHeight[objname] -= speed;
			obj[objname].style.height = curHeight[objname] + "px";
			return;
		} else {
			obj[objname].style.display = "none";
			obj[objname].style.height = "";
		}
	} else {										// Opening, stop at offsetHeight of enclosed <span>
		if(curHeight[objname] < tobj[objname].offsetHeight) {
			curHeight[objname] += speed;
			obj[objname].style.height = curHeight[objname] + "px";
			return;
		} else {
			obj[objname].style.display = "block";
			obj[objname].style.height = "";
		}
	}
	clearInterval(timerID[objname]);				// Done, stop timer, etc.
	delete(moving[objname]);
	delete(timerID[objname]);
	delete(startTime[objname]);
	delete(tobj[objname]);
	delete(obj[objname]);
	delete(dir[objname]);
}

function toggleSlide(objname) {
	if(moving[objname]) return;
	moving[objname] = true;
	obj[objname] = document.getElementById(objname); 
	tobj[objname] = document.getElementById("t" + objname);
	if(obj[objname].style.display == "none") {
		curHeight[objname] = 1;
		obj[objname].style.height = "1px";
		dir[objname] = "down";
	} else {
		dir[objname] = "up";
	}
	startTime[objname] = (new Date()).getTime();
	obj[objname].style.display = "block";
	timerID[objname] = setInterval('slidetick(\'' + objname + '\');', timerlen);
}

