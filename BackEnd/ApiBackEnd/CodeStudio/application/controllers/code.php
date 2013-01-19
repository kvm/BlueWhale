<?php 
//
//if ( ! defined('BASEPATH')) exit('No direct script access allowed');
require(APPPATH.'/libraries/REST_Controller.php'); 
class Code extends REST_Controller {


	private function dbConnect()
	{
		//echo $server;
		
		//$server='localhost';
		//echo $server;
		$user='root';
		$password='';
		$db='test';
		$this->con = mysql_connect($server,$user,$password,$db);
		//echo $this->con;
		//mysql_select_db("outlok8y_Test", $this->con);
		//$result=mysql_query("SHOW TABLES");
		//$row=mysql_fetch_array($result);
		//echo $row;
		if (!$this->con)
		{
			$this->response('',500);
		}		
	}
	
	function __construct()
	{
		parent::__construct();
		$this->dbConnect();
	}
	public function index_get()
	{
		echo 'welcome';
		$client = new SoapClient("http://ideone.com/api/1/service.wsdl");
		$testArray = $client->testFunction("test", "test");
	
	// printing returned values
	echo "<table>\n";
	echo "<tr><th>key</th><th>value</th><th>string</th><th>float</th><th>integer</th><th>bool</th></tr>\n";
	foreach($testArray as $k => $v) {
		echo "<td>" . $k . "</td><td>" . $v
			. "</td><td>" . is_string($v)
			. "</td><td>" . is_float($v)
			. "</td><td>" . is_integer($v)
			. "</td><td>" . is_bool($v)
			. "</td><td>\n";
		echo "</tr>\n";
	}
	echo "</table>\n";
	$result = $client->getLanguages('CodeStudio','forceindia');
	var_dump($result);
	echo $result["languages"][1];
	}
	//
	public function submit_get($code)
	{
		echo 'get';
		$client = new SoapClient("http://ideone.com/api/1/service.wsdl");
		
		$code = '#include<stdio.h> 
		int main() { printf("i am ankur"); return 0 ;}';
		//$client = new SoapClient("http://ideone.com/api/1/service.wsdl");
		$codeID = $client->createSubmission('CodeStudio','forceindia',$code,1,'',true,0);
		var_dump($codeID); 
	}
	
	
	public function submit_post()
	{
		echo 'post';
	
	}
}

/* End of file welcome.php */
/* Location: ./application/controllers/welcome.php */