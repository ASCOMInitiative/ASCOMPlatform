//** Dynamic Drive Equal Columns Height script v1.01 (Nov 2nd, 06)
// Now passes JSLint (Bob Denny 30-May-2007)
var ddequalcolumns=new Object();
ddequalcolumns.columnswatch=["leftcolumn", "rightcolumn", "contentwrapper"];
ddequalcolumns.setHeights=function(reset)
{
    var tallest=0;
    var resetit=(typeof reset=="string")? true : false;
    for (var i=0; i<this.columnswatch.length; i++)
    {
        if (document.getElementById(this.columnswatch[i])!==null)
        {
            if (resetit)
                document.getElementById(this.columnswatch[i]).style.height="auto";
            if (document.getElementById(this.columnswatch[i]).offsetHeight>tallest)
                tallest=document.getElementById(this.columnswatch[i]).offsetHeight;
        }
    }
    if (tallest>0)
    {
        for (i=0; i<this.columnswatch.length; i++)
        {
            if (document.getElementById(this.columnswatch[i])!==null)
                document.getElementById(this.columnswatch[i]).style.height=tallest+"px";
        }
    }
};
ddequalcolumns.resetHeights=function()
{
    this.setHeights("reset");
};
ddequalcolumns.dotask=function(target, functionref, tasktype)
{
    var _tasktype=(window.addEventListener)? tasktype : "on"+tasktype;
    if (target.addEventListener)
        target.addEventListener(_tasktype, functionref, false);
    else if (target.attachEvent)
        target.attachEvent(_tasktype, functionref);
};
ddequalcolumns.dotask(window, function()
{
    ddequalcolumns.setHeights();
}
, "load");
ddequalcolumns.dotask(window, function()
{
    if (typeof ddequalcolumns.timer!="undefined") clearTimeout(ddequalcolumns.timer);
        ddequalcolumns.timer=setTimeout("ddequalcolumns.resetHeights()", 200);
}
, "resize");
