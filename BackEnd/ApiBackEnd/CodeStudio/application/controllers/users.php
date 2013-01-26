<?php
//This will incluse the Following files. If these are not checked in , it will produce fatal error and halt the script
require "Responses/CheckUser.php";
require "Config/Config.php";
require(APPPATH.'/libraries/REST_Controller.php'); 
require('Config/Config.php');
class users extends REST_Controller {
	private $con;
	private $logger;
	private $mySqlError;
	private $mySqlLogError;
	function __construct()
	{
		parent::__construct();
		$this->dbConnect();
	}
	
	/*function __destruct() {
       
	   print "Destroying " . $this->name . "\n";
   }*/
	
	//Connect To DataBase , Establish Connections
   private function dbConnect()
	{
	
		/*$server='199.79.62.115';
		$user='outlok8y_super';
		$password='forceindia';
		$db='outlok8y_Test';*/
		$server='localhost';
		$user='root';
		$password='';
		$db='test';
		$this->con = mysql_connect($server,$user,$password,$db);
		mysql_select_db('outlok8y_Test', $this->con);
		/*
		Debugging Code
		$db_list = mysql_list_dbs($con);
		$i = 0;
		$cnt = mysql_num_rows($db_list);
		while ($i < $cnt) {
			echo mysql_db_name($db_list, $i) . "\n";
			$i++;
		}
		$r = mysql_query("INSERT INTO `outlok8y_Test`.`CSuser` (`ID`, `ServiceUserName`, `ServiceUserID`, `ServiceType`, `IdeOneID`, `FirstName`, `LastName`) VALUES ('3', '12', '12', '1', '12', 'asd', 'dasd')");// or die(mysql_error());
		$result=mysql_query("SELECT DATABSAE()");
		echo mysql_error();
		echo mysql_result($r,0);
		
		*/	

		if (!$this->con)
		{
			echo 'Not done';
		}		
	}
	
	//checking internally if user exists
	private function CheckUserExists( $serviceUserId , $serviceType, $CallFrom)
	{
		//Getting the Result From Server
		$sql='SELECT COUNT(*) AS num from CSuser where ServiceUserID ='.$serviceUserId.' AND serviceType='.$serviceType.'';
		$result = mysql_query($sql,$this->con);		
		
		//Logging the Query Into The server.
		
		if($result)
		  {
			$row = mysql_fetch_array($result);
			if($row['num']>0)
			{
			$log_query = 'Insert INTO Logs VALUES("'.$CallFrom.'",UTC_TIMESTAMP(),2,"'.$serviceUserId.' '.$serviceType.' was checked for existence with count '.$row['num'].'")';
			if(!mysql_query($log_query,$this->con))
			$this->mySqlLogError =  mysql_error();			
			return 1;
			}
			else return 0;
		  }
		else
		{
			$this->mySqlError = mysql_error();
			$log_query = 'Insert INTO Logs VALUES("'.$CallFrom.'",UTC_TIMESTAMP(),1,"Error while executing sql:'.mysql_error().')';			
			$this->mySqlLogError = mysql_error();
			return -1;
		}		
	}
	
	//Debug Function
	public function Debug_get()
	{
		$this->response(array("Hey , Nothing here"=>"Absolutely Nothing"),200);
	}
	
	//Check if user exists . Public Function exposed via API
	public function CheckUser_get( $serviceUserId , $serviceType)
	{
		$retValue = $this->CheckUserExists( $serviceUserId , $serviceType,'CheckUser');
		
		if( $retValue == 1)
		{
			$this->response(array("IsPresent"=>1,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->mySqlLogError.":")),200);
		}
		if($retValue == 0)
		{
			$this->response(array("IsPresent"=>0,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->mySqlLogError.":")),200);
		}
		$this->response(array("Error"=>mysql_error()),500);
	}
	
	//Insert a new user
	public function CreateUser_post($serviceUserId,$Username,$firstName,$LastName,$ServiceType,$debugMode)
	{
		$ideoneAccounts=1;		
		$checkUser = $this->CheckUserExists( $serviceUserId , $ServiceType,'CreateUser');
		if($debugMode == 1) 
		{
			$checkUser = 0;
		}
		if($checkUser ==1)
		{
			$this->response(array("IsPresent"=>1,"Created"=>0,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->mySqlLogError.":")),200);
		}
		
		if($checkUser == -1)
		{
			$this->response(array("IsPresent"=>-1,"Created"=>-1,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->mySqlLogError.":")),500);;
		}
		
		$sql='INSERT INTO `outlok8y_Test`.`CSuser`
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
		
		$queryResult = mysql_query($sql,$this->con);
		$this->mySqlError = mysql_error();
		if($queryResult)
		  {			
			$log_query = 'Insert INTO Logs VALUES("CreateUser_post",UTC_TIMESTAMP(),1,"'.$Username.' '.$serviceUserId.' was created")';
			$log_query_result = mysql_query($log_query,$this->con);
			if(!$log_query_result)
			$this->$mySqlLogError = mysql_error();
			$this->response(array("IsPresent"=>0,"Created"=>1,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->mySqlLogError.":")),200);
		  }
		else
		{	
			$log_query = 'Insert INTO Logs VALUES("CreateUser_post",UTC_TIMESTAMP(),1,"User Creation Failed was created")';
			$log_query_result = mysql_query($log_query,$this->con);
			if(!$log_query_result)
			$this->$mySqlLogError = mysql_error();
			$this->response(array("IsPresent"=>-1,"Created"=>-1,"Error"=>($this->mySqlError),"LoggingError"=>(":".$this->$mySqlLogError.":")),500);;
		}		
		//mysql_close($this->con);
		//var_dump($_POST);
		mysql_close($this->con);	
	}

}
?>
