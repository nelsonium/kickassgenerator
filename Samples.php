<?php
  session_start();
  $token = md5(rand(1000,9999)); //you can use any encryption
  $_SESSION['token'] = $token; //store it as session variable
?>

<?php



if($_SERVER['HTTP_X_REQUESTED_WITH'] == 'XMLHttpRequest' && isset($_POST['token']) && $_POST['token'] === $_SESSION['token'])
{ 
	if(@isset($_SERVER['HTTP_REFERER']) && $_SERVER['HTTP_REFERER']=="http://yourdomain/ajaxurl")
  {
    
$json = '
{
    "type": "donut",
    "name": "Cake"
}';
$jon = $_GET["JSONObjectHere"];
$data = json_decode($json);

echo $data->type;



  }
}
?>