# Cache-Assignment
**How to run:** Type CTRL+F5 in your Visual Studio IDE. Then go to file "Cache-Assignment.http" and send your desired requests (you will be able to receive succesful responses only after 4 POST requests).

**Example request flow:**
=== REQUEST ===
POST https://localhost:7164/api/products HTTP/1.1
Content-Type: application/json

{
}

=== RESPONSE ===
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: https://localhost:7164/api/Products/5

{"id":5}

**Example demonstrating cache hit/miss:** Simply send 2 GET requests by less/more than 3 seconds delay.

**Key design considerations:** By adopting an (atomicly) incrementive number, I chose the 'long' type in order to prevent any chance of id duplications.
