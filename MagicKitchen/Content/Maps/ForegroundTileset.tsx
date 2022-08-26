<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.0" name="ForegroundTileset" tilewidth="16" tileheight="16" tilecount="10000" columns="100">
 <image source="Foreground2.png" width="1600" height="1600"/>
 <tile id="233">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3400">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="3602">
  <properties>
   <property name="EmitOffSet" value="0,-26"/>
   <property name="animate" value="pause"/>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="furniture" value="CraftingTable"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="3602" duration="50"/>
   <frame tileid="3902" duration="50"/>
   <frame tileid="4202" duration="50"/>
   <frame tileid="4502" duration="50"/>
  </animation>
 </tile>
 <tile id="3604">
  <properties>
   <property name="IconType" value="Break"/>
   <property name="animate" value="pause"/>
   <property name="furniture" value="StorableFurniture,2,4"/>
   <property name="newHitBox" value="0,0,16,16"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <animation>
   <frame tileid="3604" duration="100"/>
   <frame tileid="3606" duration="100"/>
   <frame tileid="3608" duration="100"/>
   <frame tileid="3610" duration="100"/>
   <frame tileid="3612" duration="100"/>
  </animation>
 </tile>
 <tile id="3606">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="3608">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="3610">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="3612">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="3728">
  <properties>
   <property name="lightSource" value=".6,-8,8,Warm,immune:false"/>
   <property name="newSource" value="-16,-32,32,48"/>
  </properties>
 </tile>
 <tile id="3768">
  <properties>
   <property name="newSource" value="-16,-48,48,64"/>
  </properties>
  <animation>
   <frame tileid="3768" duration="800"/>
   <frame tileid="4168" duration="800"/>
   <frame tileid="4568" duration="800"/>
   <frame tileid="4168" duration="800"/>
  </animation>
 </tile>
 <tile id="3866">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <animation>
   <frame tileid="3866" duration="800"/>
   <frame tileid="4066" duration="800"/>
   <frame tileid="4266" duration="800"/>
   <frame tileid="4066" duration="800"/>
  </animation>
 </tile>
 <tile id="3902">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="3904">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="furniture" value="Grill"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3926">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4066">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="4168">
  <properties>
   <property name="newSource" value="-16,-48,48,64"/>
  </properties>
 </tile>
 <tile id="4202">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="4266">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="4362">
  <animation>
   <frame tileid="4362" duration="800"/>
   <frame tileid="4462" duration="800"/>
   <frame tileid="4562" duration="800"/>
   <frame tileid="4462" duration="800"/>
  </animation>
 </tile>
 <tile id="4502">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="4568">
  <properties>
   <property name="newSource" value="-16,-48,48,64"/>
  </properties>
 </tile>
 <tile id="5412">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5414">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5416">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5418">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5812">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5814">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingKey" value="wall,land,tall"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5816">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5818">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5820">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5822">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5824">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="5826">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="6212">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="6214">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
 <tile id="6216">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
 </tile>
</tileset>
