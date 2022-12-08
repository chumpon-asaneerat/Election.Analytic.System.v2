CREATE VIEW ADMView
AS
	SELECT A.ADM1Code, C.ProvinceName
		 , A.ADM2Code, B.DistrictName
		 , A.ADM3Code, A.SubdistrictName
	  FROM LSubdistrict A 
	  LEFT JOIN LDistrict B ON (B.ADM1Code = A.ADM1Code AND B.ADM2Code = A.ADM2Code)
	  LEFT JOIN LProvince C ON (C.ADM1Code = A.ADM1Code)
	 ORDER BY C.ProvinceName, B.DistrictName, A.SubdistrictName
