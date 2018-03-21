<b>Blissmo</b> is a study and concept of incorporating a micro-service based architecture for an online booking/reservation web application. Please refer wiki section for further information on this study.

<h2>Getting Started</h2>
<h3>Set up development environment</h3>
<ul>
	<li><a target="_blank" href="https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started#to-use-visual-studio-		2017">Install the Service Fabric runtime, SDK, and tools</a></li>
	</br>
	<li>Enable PowerShell script execution
	<pre>Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force -Scope CurrentUser</pre>
	</li>
</ul>
    
<h3>Configure application settings</h3>
Blissmo uses Azure Search Service, RabbitMq and SendGrid. Therefore, you have to configure application settings from \Blissmo\ApplicationPackageRoot\ApplicationManifest.xml file.

<h3>High-Level Architecture Design</h3>
<img src="https://github.com/Tiqri/sf/blob/master/blissmo_high_level_diagram.png" />
