# Diff

To use the api you need to start the project first. When api is running you can use Swagger. Api is using memory cache, and does not need any additional db setup on your machine.

diff/{id}/left - Saves the left part of the diff
diff/{id}/right - Saves the right part of the diff

In both cases, if that side of the diff doesn't exist yet, it will create a new entry, if it does it will update it.

diff/get/{id} - gets the diff and can return different responses

If diff under that id doesn't exist it will return NotFound
If left or right is empty, it should return BadRequest
If they are equal it will return Ok("Diffs are equal")
If they are not of equal size it will return Ok("Input not of equal size");

Otherwise, it will return the diff location and length of the diff
