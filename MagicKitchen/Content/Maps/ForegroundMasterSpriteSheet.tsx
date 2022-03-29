<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.3.0" name="ForegroundMasterSpriteSheet" tilewidth="16" tileheight="16" tilecount="5625" columns="75">
 <image source="ForegroundMasterSpriteSheet.png" width="1200" height="1200"/>
 <tile id="87">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="0"/>
  </properties>
 </tile>
 <tile id="88">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="1"/>
  </properties>
 </tile>
 <tile id="89">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="2"/>
  </properties>
 </tile>
 <tile id="90">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="3"/>
  </properties>
 </tile>
 <tile id="91">
  <properties>
   <property name="newSource" value="0,-16,16,32"/>
   <property name="replace" value="4"/>
  </properties>
 </tile>
 <tile id="451">
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
 <tile id="826">
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
 <tile id="1439">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="destructable" value="Rock"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1440" duration="100"/>
   <frame tileid="1441" duration="100"/>
   <frame tileid="1442" duration="100"/>
   <frame tileid="1443" duration="100"/>
   <frame tileid="1444" duration="100"/>
  </animation>
 </tile>
 <tile id="1514">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="destructable" value="Rock"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1590" duration="100"/>
   <frame tileid="1591" duration="100"/>
   <frame tileid="1592" duration="100"/>
   <frame tileid="1593" duration="100"/>
  </animation>
 </tile>
 <tile id="1515">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="destructable" value="Rock"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1590" duration="100"/>
   <frame tileid="1592" duration="100"/>
   <frame tileid="1593" duration="100"/>
   <frame tileid="1591" duration="100"/>
  </animation>
 </tile>
 <tile id="1589">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="destructable" value="Rock"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1590" duration="100"/>
   <frame tileid="1591" duration="100"/>
   <frame tileid="1592" duration="100"/>
   <frame tileid="1593" duration="100"/>
  </animation>
 </tile>
 <tile id="1664">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="16" height="16">
    <properties>
     <property name="destructable" value="Rock"/>
    </properties>
    <ellipse/>
   </object>
  </objectgroup>
  <animation>
   <frame tileid="1665" duration="100"/>
   <frame tileid="1666" duration="100"/>
   <frame tileid="1667" duration="100"/>
   <frame tileid="1668" duration="100"/>
   <frame tileid="1669" duration="100"/>
  </animation>
 </tile>
 <tile id="2778">
  <properties>
   <property name="newHitBox" value="-48, -80, 124, 96"/>
   <property name="newSource" value="-48, -124, 136,144"/>
   <property name="portal" value="true,Town,Restaurant,48,0,32,32,0,48,Up"/>
  </properties>
 </tile>
 <tile id="4296">
  <properties>
   <property name="action" value="ignite"/>
   <property name="newHitBox" value="4, -8, 26"/>
   <property name="newSource" value="0, -32, 32, 48"/>
  </properties>
 </tile>
</tileset>
