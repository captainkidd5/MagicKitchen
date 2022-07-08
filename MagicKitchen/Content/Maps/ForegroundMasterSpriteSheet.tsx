<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.0" name="ForegroundMasterSpriteSheet" tilewidth="16" tileheight="16" tilecount="10000" columns="100">
 <image source="ForegroundMasterSpriteSheet.png" width="1600" height="1600"/>
 <tile id="112">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="0"/>
  </properties>
 </tile>
 <tile id="113">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="1"/>
  </properties>
 </tile>
 <tile id="114">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="2"/>
  </properties>
 </tile>
 <tile id="115">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="3"/>
  </properties>
 </tile>
 <tile id="116">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="4"/>
  </properties>
 </tile>
 <tile id="233">
  <properties>
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
   <frame tileid="233" duration="100"/>
   <frame tileid="533" duration="100"/>
   <frame tileid="833" duration="100"/>
   <frame tileid="1133" duration="100"/>
  </animation>
 </tile>
 <tile id="533">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="571">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="572">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="574">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="575">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="577">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="601">
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
 <tile id="771">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="772">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="774">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="775">
  <properties>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="777">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="833">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="940">
  <properties>
   <property name="IconType" value="Break"/>
   <property name="animate" value="pause"/>
   <property name="furniture" value="StorableFurniture,2,4"/>
   <property name="newHitBox" value="0,0,16,16"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <animation>
   <frame tileid="940" duration="100"/>
   <frame tileid="941" duration="100"/>
   <frame tileid="942" duration="100"/>
   <frame tileid="943" duration="100"/>
   <frame tileid="944" duration="100"/>
  </animation>
 </tile>
 <tile id="941">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="942">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="943">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="944">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="971">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0" width="13" height="13">
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="971" duration="800"/>
   <frame tileid="771" duration="800"/>
   <frame tileid="571" duration="800"/>
   <frame tileid="771" duration="800"/>
  </animation>
 </tile>
 <tile id="972">
  <properties>
   <property name="newHitBox" value="0,-16,32,32"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
  <animation>
   <frame tileid="972" duration="800"/>
   <frame tileid="772" duration="800"/>
   <frame tileid="572" duration="800"/>
   <frame tileid="772" duration="800"/>
  </animation>
 </tile>
 <tile id="974">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="0" width="13" height="13">
    <properties>
     <property name="action" value="Harvest,971"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="974" duration="800"/>
   <frame tileid="774" duration="800"/>
   <frame tileid="574" duration="800"/>
   <frame tileid="774" duration="800"/>
  </animation>
 </tile>
 <tile id="975">
  <properties>
   <property name="action" value="Harvest,972"/>
   <property name="newHitBox" value="0,-16,32,32"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
  <animation>
   <frame tileid="975" duration="800"/>
   <frame tileid="775" duration="800"/>
   <frame tileid="575" duration="800"/>
   <frame tileid="775" duration="800"/>
  </animation>
 </tile>
 <tile id="977">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="977" duration="800"/>
   <frame tileid="777" duration="800"/>
   <frame tileid="577" duration="800"/>
   <frame tileid="777" duration="800"/>
  </animation>
 </tile>
 <tile id="1101">
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
 <tile id="1109">
  <properties>
   <property name="newSource" value="-16,-64,48,80"/>
   <property name="transparent" value="-16,-48, 48, 60"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="2" y="4" width="13" height="11">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="1133">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="1279">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1280">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1281">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1332">
  <properties>
   <property name="furniture" value="CraftingTable"/>
   <property name="newHitBox" value="0,-8,32,24"/>
   <property name="newSource" value="0,-16,32,32"/>
  </properties>
 </tile>
 <tile id="1476">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1477">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingKey" value="woodWall,,tall"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1478">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1547">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="1774">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1775">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1779">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1780">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1781">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1782">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1783">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1847">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
 </tile>
 <tile id="1914">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1915" duration="100"/>
   <frame tileid="1916" duration="100"/>
   <frame tileid="1917" duration="100"/>
   <frame tileid="1918" duration="100"/>
   <frame tileid="1919" duration="100"/>
  </animation>
 </tile>
 <tile id="1976">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1977">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="1978">
  <properties>
   <property name="newSource" value="0,-48,16,64"/>
   <property name="tilingSet" value="woodWall"/>
  </properties>
 </tile>
 <tile id="2014">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="2115" duration="100"/>
   <frame tileid="2116" duration="100"/>
   <frame tileid="2117" duration="100"/>
   <frame tileid="2118" duration="100"/>
  </animation>
 </tile>
 <tile id="2015">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="2115" duration="100"/>
   <frame tileid="2117" duration="100"/>
   <frame tileid="2118" duration="100"/>
   <frame tileid="2116" duration="100"/>
  </animation>
 </tile>
 <tile id="2114">
  <objectgroup draworder="index" id="2">
   <object id="2" x="5" y="3">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <polygon points="0,0 9,2 9,8 1,9"/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="2115" duration="100"/>
   <frame tileid="2116" duration="100"/>
   <frame tileid="2117" duration="100"/>
   <frame tileid="2118" duration="100"/>
  </animation>
 </tile>
 <tile id="2124">
  <properties>
   <property name="lightSource" value="2,8,8,Nautical"/>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="3" y="9" width="5" height="5">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="2131">
  <properties>
   <property name="lightSource" value="3,8,8,Warm"/>
   <property name="newHitBox" value="8,0,16,16"/>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="2147">
  <properties>
   <property name="newSource" value="0,-32,16,48"/>
  </properties>
  <animation>
   <frame tileid="2147" duration="300"/>
   <frame tileid="1847" duration="300"/>
   <frame tileid="1547" duration="300"/>
  </animation>
 </tile>
 <tile id="2214">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="action" value="Break,Hammer,Good"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="2215" duration="100"/>
   <frame tileid="2216" duration="100"/>
   <frame tileid="2217" duration="100"/>
   <frame tileid="2218" duration="100"/>
   <frame tileid="2219" duration="100"/>
  </animation>
 </tile>
 <tile id="3703">
  <properties>
   <property name="newHitBox" value="-48, -80, 128, 96"/>
   <property name="newSource" value="-48, -124, 128,144"/>
   <property name="portal" value="true,LullabyTown,Restaurant,48,0,32,32,0,48,Up"/>
   <property name="transparency" value="-48, -124, 128,128"/>
  </properties>
 </tile>
 <tile id="3708">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="2" x="5" y="9" width="6" height="6">
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="3708" duration="100"/>
   <frame tileid="3709" duration="100"/>
  </animation>
 </tile>
 <tile id="3709">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
  </properties>
 </tile>
 <tile id="4021">
  <properties>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
 </tile>
 <tile id="4321">
  <properties>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
 </tile>
 <tile id="4621">
  <properties>
   <property name="newHitBox" value="4, -8, 26"/>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
  <animation>
   <frame tileid="4621" duration="100"/>
   <frame tileid="4321" duration="100"/>
   <frame tileid="4021" duration="100"/>
  </animation>
 </tile>
 <tile id="4921">
  <properties>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
 </tile>
 <tile id="5321">
  <properties>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
 </tile>
 <tile id="5721">
  <properties>
   <property name="IconType" value="Ignite"/>
   <property name="action" value="Ignite"/>
   <property name="newHitBox" value="4, -8, 26"/>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
  <animation>
   <frame tileid="5321" duration="200"/>
   <frame tileid="4921" duration="200"/>
   <frame tileid="4621" duration="200"/>
   <frame tileid="4321" duration="100"/>
   <frame tileid="4021" duration="100"/>
  </animation>
 </tile>
 <tile id="6822">
  <properties>
   <property name="newHitBox" value="0,8,32,8"/>
   <property name="newSource" value="0,-32,32,48"/>
  </properties>
 </tile>
 <tile id="8259">
  <properties>
   <property name="newHitBox" value="(-176,-280), (64,224), (130,204), (294,210), (312,254), (314,280), (308,288), (276,306), (294,210), (250,296), (106,308), (74,284), (62,258)"/>
   <property name="newSource" value="-160,-296,368,320"/>
   <property name="portal" value="true,LullabyTown,Restaurant,0,32,32,32,0,48,Up"/>
  </properties>
 </tile>
</tileset>
