# APIs' Design

## Kingdom

- GET /kingdom
- POST /kingdom
- PUT /kingdom
- DELETE /kingdom

- GET /kingdom/{kingdomId}

- GET /kingdom/{kingdomId}/clan
- POST /kingdom/{kingdomId}/clan

- GET /kingdom/{kingdomId}/nobleman
- POST /kingdom/{kingdomId}/nobleman

- GET /kingdom/{kingdomId}/responsibility
- POST /kingdom/{kingdomId}/responsibility

## Clan

- GET /clan/{clanId}
- PUT /clan/{clanId}
- DELETE /clan/{clanId}

- POST /clan/{clanId}/add-noblemen
- POST /clan/{clanId}/remove-noblemen
- POST /clan/{clanId}/add-responsibilities
- POST /clan/{clanId}/remove-responsibilities

- GET /clan/{clanId}/nobleman
- GET /clan/{clanId}/responsibility

## Nobleman

- GET /nobleman/{noblemanId}
- PUT /nobleman/{noblemanId}
- DELETE /nobleman/{noblemanId}

- GET /nobleman/{noblemanId}/responsibility

- POST /nobleman/{noblemanId}/add-responsibilities
- POST /nobleman/{noblemanId}/remove-responsibilities

## Responsibility

- GET /responsibility/{responsibility}
- PUT /responsibility/{responsibility}
- DELETE /responsibility/{responsibility}
