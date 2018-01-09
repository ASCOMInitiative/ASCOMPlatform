add-pssnapin ascom.powershell.cmdlets
$id=show-chooser -deviceid "scopesim.telescope"
$id
$scope=new-telescope $id
$scope
