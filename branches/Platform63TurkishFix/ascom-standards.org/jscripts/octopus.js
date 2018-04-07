// Javascript by Gilbert Hyatt
// http://pajaj.sourceforge.net
// In Association with Dragon Labs & Octopus Engine
// http://dragon-labs.com/articles/octopus
//
// Now passes JSLint (R. Denny 29-May-2007)
//
function addEvent(obj, evType, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(evType, fn, true);
        return true;
    } else if (obj.attachEvent) {
        var r = obj.attachEvent("on"+evType, fn);
        return r;
    } else {
        return false;
    }
}

var initOctopusDone = false;

function initOctopus() {
    if (initOctopusDone) { return true; }

    var classTree     = new Array(3);
    classTree[0]  = ["north","east","south","west","ne","se","sw","nw"];
    classTree[1]  = ["faux","north","south"];
    classTree[2]  = ["north", "east", "west", "south"];
    var classNames    = ['octopus', 'squid', 'swordfish'];
   
    var tempdivs = [];
    var divs = document.getElementsByTagName('div');
    for (var i=0;i<divs.length;i++) {
        for (var j=0; j<3; j++) {
            var cdiv = divs[i];
            if (cdiv.className.indexOf(classNames[j]) > -1) {
                var tempinner = cdiv.innerHTML;
                cdiv.innerHTML = "";
                var prevdiv = cdiv;
                for (var a=0; a<classTree[j].length; a++) {
                    tempdivs[a]           = document.createElement('div');
                    tempdivs[a].className = classTree[j][a];
                    prevdiv.appendChild(tempdivs[a]);
                    prevdiv = tempdivs[a];
                }
                prevdiv.innerHTML = tempinner;
            }
        }
    }
    initOctopusDone = true;
    return 0;
}

if (document.getElementById && document.createElement) {
    addEvent(window, 'load', initOctopus);
}