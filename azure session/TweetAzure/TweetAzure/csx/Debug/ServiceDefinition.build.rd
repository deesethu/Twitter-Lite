<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="TweetAzure" generation="1" functional="0" release="0" Id="cf968467-b4ef-46e8-9dc4-4f934148414c" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TweetAzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WCFServiceWebRole1:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/TweetAzure/TweetAzureGroup/LB:WCFServiceWebRole1:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/TweetAzure/TweetAzureGroup/LB:WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapCertificate|WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="WCFServiceWebRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/TweetAzure/TweetAzureGroup/MapWCFServiceWebRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WCFServiceWebRole1:Endpoint1">
          <toPorts>
            <inPortMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapWCFServiceWebRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WCFServiceWebRole1" generation="1" functional="0" release="0" software="C:\Users\MRND23\Desktop\tweet With Azure dev Fabric\TweetAzure\TweetAzure\csx\Debug\roles\WCFServiceWebRole1" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/TweetAzure/TweetAzureGroup/SW:WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WCFServiceWebRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WCFServiceWebRole1&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="WCFServiceWebRole1.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1Instances" />
            <sCSPolicyFaultDomainMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="WCFServiceWebRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WCFServiceWebRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a4b00a97-4f8e-4fa8-b5d9-2adf0d07299a" ref="Microsoft.RedDog.Contract\ServiceContract\TweetAzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="1cb49c82-f848-4201-9ed4-ac5411b90d09" ref="Microsoft.RedDog.Contract\Interface\WCFServiceWebRole1:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="dbabc7f0-93cd-4e84-b54d-3ac6f377e079" ref="Microsoft.RedDog.Contract\Interface\WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/TweetAzure/TweetAzureGroup/WCFServiceWebRole1:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>