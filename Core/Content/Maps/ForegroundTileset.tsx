<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.0" name="ForegroundTileset" tilewidth="16" tileheight="16" tilecount="10000" columns="100">
 <image source="Foreground2.png" width="1600" height="1600"/>
 <tile id="233">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <ellipse/>
   </object>
   <object id="3" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="602">
  <properties>
   <property name="newSource" value="-16,-80,48,96"/>
   <property name="transparent" value="-16,-64, 48, 75"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="733">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="1"/>
  </properties>
 </tile>
 <tile id="734">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="4"/>
  </properties>
 </tile>
 <tile id="735">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="2"/>
  </properties>
 </tile>
 <tile id="736">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="3"/>
  </properties>
 </tile>
 <tile id="737">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="4"/>
  </properties>
 </tile>
 <tile id="1102">
  <properties>
   <property name="newSource" value="-16,-64,48,80"/>
   <property name="transparent" value="-16,-48, 48, 60"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
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
   <property name="animationSounds" value="0:HatchClose"/>
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
   <property name="animationSounds" value="0:HatchClose"/>
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
 <tile id="3726">
  <properties>
   <property name="newSource" value="-16,-48,32,64"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" x="9" y="9" width="6" height="6">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3728">
  <properties>
   <property name="lightSource" value=".6,4,14,Warm,immune:false"/>
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
 <tile id="4113">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4115">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4117">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="4119">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
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
 <tile id="4230">
  <properties>
   <property name="lightSource" value=".3,8,8,Nautical,immune:true"/>
   <property name="newHitBox" value="4, -8, 26"/>
   <property name="newSource" value="0,-64,32,80"/>
  </properties>
  <animation>
   <frame tileid="4230" duration="100"/>
   <frame tileid="4232" duration="100"/>
   <frame tileid="4234" duration="100"/>
   <frame tileid="4236" duration="100"/>
   <frame tileid="4238" duration="100"/>
  </animation>
 </tile>
 <tile id="4232">
  <properties>
   <property name="newSource" value="0,-64,32,80"/>
  </properties>
 </tile>
 <tile id="4234">
  <properties>
   <property name="newSource" value="0,-64,32,80"/>
  </properties>
 </tile>
 <tile id="4236">
  <properties>
   <property name="newSource" value="0,-64,32,80"/>
  </properties>
 </tile>
 <tile id="4238">
  <properties>
   <property name="newSource" value="0,-64,32,80"/>
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
 <tile id="4513">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4515">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingKey" value="stone_wall,land,tall"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4517">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4519">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4521">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4523">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4525">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4527">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4568">
  <properties>
   <property name="newSource" value="-16,-48,48,64"/>
  </properties>
 </tile>
 <tile id="4803">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="4" y="0" width="13" height="14">
    <ellipse/>
   </object>
   <object id="2" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="4804">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="14"/>
   <object id="2" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="4805">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="0" width="12" height="13">
    <ellipse/>
   </object>
   <object id="3" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="4806">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" x="4" y="0" width="8" height="15">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="4" x="4" y="0" width="8" height="15"/>
  </objectgroup>
 </tile>
 <tile id="4913">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4915">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4917">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="stone_wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="4987">
  <properties>
   <property name="newSource" value="-32,-208,96,224"/>
  </properties>
 </tile>
 <tile id="5003">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="3" y="0" width="14" height="14">
    <ellipse/>
   </object>
   <object id="2" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="5004">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" x="0" y="0" width="16" height="15"/>
   <object id="5" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="5005">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="14" height="14">
    <ellipse/>
   </object>
   <object id="2" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="5006">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="tilingKey" value="woodFence,land,fence"/>
   <property name="tilingSet" value="woodFence"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="4" y="8" width="8" height="8">
    <ellipse/>
   </object>
   <object id="2" x="4" y="0" width="9" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
 <tile id="5076">
  <properties>
   <property name="newSource" value="-16,-240,160,256"/>
  </properties>
 </tile>
 <tile id="5141">
  <properties>
   <property name="newSource" value="0,-96,224,112"/>
  </properties>
 </tile>
 <tile id="5231">
  <properties>
   <property name="newHitBox" value="0,-16,32,32"/>
   <property name="newSource" value="0,-80,32,96"/>
  </properties>
 </tile>
 <tile id="5412">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5414">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="6" y="0" width="10" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5416">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5418">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="10" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5535">
  <properties>
   <property name="newSource" value="0,-48,32,64"/>
  </properties>
 </tile>
 <tile id="5629">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="5812">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="6" y="0" width="10" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5814">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingKey" value="wall,land,tall"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="1" y="0" width="15" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5816">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="0" width="10" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5818">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5820">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="6" y="0" width="4" height="16"/>
  </objectgroup>
 </tile>
 <tile id="5822">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="6">
   <object id="5" x="4" y="8" width="8" height="8">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="5824">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="1" y="9" width="15" height="7"/>
  </objectgroup>
 </tile>
 <tile id="5826">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="4">
   <object id="3" x="6" y="-1" width="4" height="17"/>
  </objectgroup>
 </tile>
 <tile id="5834">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="5835">
  <properties>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="5839">
  <properties>
   <property name="newHitBox" value="0,8,32,8"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="5929">
  <properties>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="5931">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="6133">
  <properties>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="6135">
  <properties>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="6212">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="5" y="0" width="11" height="16"/>
  </objectgroup>
 </tile>
 <tile id="6214">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16"/>
  </objectgroup>
 </tile>
 <tile id="6216">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
   <property name="tilingSet" value="wall"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="5" y="0" width="11" height="16"/>
  </objectgroup>
 </tile>
 <tile id="6225">
  <objectgroup draworder="index" id="2">
   <object id="1" x="10" y="10" width="6" height="6">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="6226">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="13" width="16" height="3"/>
  </objectgroup>
 </tile>
 <tile id="6227">
  <objectgroup draworder="index" id="4">
   <object id="3" x="0" y="10" width="6" height="6">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="6348">
  <properties>
   <property name="newSource" value="-16,-64,64,80"/>
  </properties>
 </tile>
 <tile id="6433">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="6435">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="6437">
  <properties>
   <property name="newHitBox" value="8,4,8,4"/>
   <property name="newSource" value="0,-48,32,64"/>
  </properties>
 </tile>
 <tile id="6440">
  <properties>
   <property name="newSource" value="0,-80,16,96"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="5" y="9" width="7" height="7">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="6520">
  <properties>
   <property name="newSource" value="0,-48,48,64"/>
  </properties>
 </tile>
 <tile id="6920">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
  </properties>
 </tile>
 <tile id="6922">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
  </properties>
 </tile>
 <tile id="7268">
  <properties>
   <property name="newSource" value="-32,-272,176,288"/>
  </properties>
 </tile>
 <tile id="7306">
  <properties>
   <property name="newSource" value="-64,-208,160,224"/>
  </properties>
 </tile>
 <tile id="7357">
  <properties>
   <property name="newSource" value="-48,-240,192,256"/>
  </properties>
 </tile>
 <tile id="7386">
  <properties>
   <property name="newSource" value="-48,-352,160,368"/>
  </properties>
 </tile>
 <tile id="7514">
  <properties>
   <property name="newSource" value="-32,-144,112,160"/>
  </properties>
 </tile>
 <tile id="7538">
  <properties>
   <property name="newSource" value="-32,-160,128,176"/>
   <property name="transparent" value="-32,-160,128,176"/>
  </properties>
 </tile>
 <tile id="7829">
  <properties>
   <property name="newHitBox" value="-48,-144,144,160"/>
   <property name="newSource" value="-48,-144,144,160"/>
  </properties>
 </tile>
 <tile id="8705">
  <properties>
   <property name="newHitBox" value="-48,-208,144,224"/>
   <property name="newSource" value="-48,-208,144,224"/>
  </properties>
 </tile>
 <tile id="8784">
  <properties>
   <property name="newSource" value="-144,-192, 160, 208"/>
  </properties>
 </tile>
 <tile id="8785">
  <properties>
   <property name="newSource" value="0,-160,144,176"/>
  </properties>
 </tile>
 <tile id="8813">
  <properties>
   <property name="newHitBox" value="-32,-208,112,224"/>
   <property name="newSource" value="-32,-208,112,224"/>
  </properties>
 </tile>
 <tile id="8820">
  <properties>
   <property name="newHitBox" value="-16,16;0,0;0,-128;136,-128;137,-63;49,-63;48,0"/>
   <property name="newSource" value="-16,-128,144,144"/>
  </properties>
 </tile>
 <tile id="8828">
  <properties>
   <property name="newHitBox" value="0,16;0,0;94,0;94,-106;0,-106"/>
   <property name="newSource" value="0,-96,96,112"/>
  </properties>
 </tile>
 <tile id="8838">
  <properties>
   <property name="newHitBox" value="-56,8;0,0;3,8;124,8;123,-92;0,-91"/>
   <property name="newSource" value="-64,-176,160,192"/>
   <property name="portal" value="TestIsland,Hallway,16,32,true,16,48"/>
  </properties>
 </tile>
 <tile id="8847">
  <properties>
   <property name="newSource" value="-32,-336,144,352"/>
  </properties>
 </tile>
 <tile id="8871">
  <properties>
   <property name="newSource" value="-272,-112,304,128"/>
  </properties>
 </tile>
</tileset>
