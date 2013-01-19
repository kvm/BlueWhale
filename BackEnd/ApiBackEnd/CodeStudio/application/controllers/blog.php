<?php
//This will incluse the Following files. If these are not checked in , it will produce fatal error and halt the script
require "Responses/CheckUser.php";
//require "Responses/LogCode.php";
//Rest codeignitoe library : https://github.com/philsturgeon/codeigniter-restserver
// http://net.tutsplus.com/tutorials/php/working-with-restful-services-in-codeigniter-2/
require(APPPATH.'/libraries/REST_Controller.php'); 
require('Config/Config.php');
class Blog extends REST_Controller {
	private $con;
	private $logger;
	function __construct()
	{
		parent::__construct();
		$this->dbConnect();
	}
	
	/*function __destruct() {
       
	   print "Destroying " . $this->name . "\n";
   }*/
	private function dbConnect()
	{
		//echo $server;
		
		$server='localhost';
		//echo $server;
		$user='root';
		$password='';
		$db='test';
		$this->con = mysql_connect($server,$user,$password,$db);
		mysql_select_db("test", $this->con);
		if (!$this->con)
		{
			$this->response('',500);
		}		
	}
	private function GetLogCode()
	{
	}
	public function CheckUser_get( $serviceUserId , $serviceType)
	{
		$error = '';
		$sql='SELECT COUNT(*) AS num from csuser where ServiceUserID ='.$serviceUserId.' AND serviceType='.$serviceType.'';
		$result = mysql_query($sql,$this->con);		

		if($result)
		  {
			$row = mysql_fetch_array($result);
			$log_query = 'Insert INTO logs VALUES("CheckUser_get",UTC_TIMESTAMP(),2,"'.$serviceUserId.' '.$serviceType.' was checked for existence with count '.$row['num'].' Error:'.$error.':")';
		//	echo $log_query;
			if(!mysql_query($log_query,$this->con))
			$error =  mysql_error();			
			if($row['num']>0)
			$this->response(array("IsPresent"=>1,"Error"=>$error),200);
			else $this->response(array("IsPresent"=>0,"Error"=>$error),200);
		  }
		else
		{
			$this->response(array("Error"=>mysql_error()),500);
			$log_query = 'Insert INTO logs VALUES("CreateUser_post",UTC_TIMESTAMP(),1,"Error while executing sql:'.mysql_error().')';
		}
		$this->response(array("Error"=>"None"),200);
	}

	public function CreateUser_post($serviceUserId,$Username,$firstName,$LastName,$ServiceType)
	{
		//echo $id;
		//echo $this->con;
		//echo $ServiceName;
		$ideoneAccounts=1;
	//	$sql = 'INSERT INTO `test`.`csuser` (ServiceUserName,ServiceUserID) VALUES ("'.$Username.'",'.$serviceUserId.')';
		
		$sql='INSERT INTO `test`.`csuser`
			(
			ServiceUserName,
			ServiceUserID,
			ServiceType,
			IdeOneID,
			FirstName,
			LastName)
			VALUES
			("'.
			 $Username.'",'.
			 $serviceUserId.','.
			 $ServiceType.','.
			rand(0,$ideoneAccounts).',"'.
			$firstName.'","'.
			$LastName.
			'")';
		if(mysql_query($sql,$this->con))
		  {
			//echo "User Database created";
			$log_query = 'Insert INTO logs VALUES("CreateUser_post",UTC_TIMESTAMP(),1,"'.$Username.' '.$serviceUserId.' was created")';
			if(!mysql_query($log_query,$this->con))
			echo mysql_error();
			
		  }
		else
		{
			echo "Error User creating database: " . mysql_error();
		}
		//mysql_close($this->con);
		//var_dump($_POST);
		mysql_close($this->con);
		$this->response(array("Error"=>"None"),201);
	}
	public function index_get()
	{
		echo 'Hello World!';
	}
	public function comments_get()
	{
		//echo 'look at the comments';
		//echo 'ankur';
		//echo '.libraries/REST_Controller.php';
		//echo APPPATH;
		$checkUser = new CheckUser();
		$checkUser->IsExisting = true;
		$checkUser->TypeOfRequest = "ankur";
		$output =array("user"=>"Apple","IsExist"=>"Tomato");
		echo $this->response($output,201);

		//echo $_SERVER['REQUEST_METHOD'];
	}

}
?>
