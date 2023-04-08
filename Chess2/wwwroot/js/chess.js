var currentMatchId
var prevElement

class Movement{
	constructor(_Id, _Piece, _StartX, _StartY, _EndX, _EndY){
		this.Id = _Id
		this.Piece = _Piece
		this.StartX = _StartX
		this.StartY = _StartY
		this.EndX = _EndX
		this.EndY = _EndY
	}
}

function squareClicked(element){
	if(prevElement == null){
		prevElement = element
		prevElement.parentElement.id="clicked"
	}

	else{
		if (currentMatchId == null) {
			return
		}
	//creates the movement object with the info of the matchId and start and end position of the piece being moved
	moving = new Movement(
		currentMatchId,
		prevElement.id,
		prevElement.parentNode.getAttribute('data-column'),
		prevElement.parentNode.parentNode.getAttribute('data-row'),
		element.parentNode.getAttribute('data-column'),
		element.parentNode.parentNode.getAttribute('data-row')
	)
	prevElement.parentElement.id = ""
	prevElement = null
	makeMove(moving)
	}
}

function requestMatch(__id = null) {
	console.log(__id+" id que llega al request")
	_id = JSON.stringify(__id)
	console.log(_id)
	$.ajax({
		type: "GET",
		url: "Chess/StartMatch",
		data: {_Id:_id},
		dataType: "json",
		success: function (data) {
			loadBoard(JSON.parse(data));
		},
	})
}
// loadBoard: load the match into the board using it's two internal fuctions to check each square
function loadBoard(Match) {
	currentMatchId = Match.Id
	document.querySelector('[name="IdDisplay"]').value = currentMatchId
	function checkCol(col) {
		rowEl = board.children[rowIndex]
		colEl = rowEl.children[colIndex]
		colEl.childNodes[0].id = col
		colIndex++
	}
	function checkRow(row) {
		colIndex = 0
		row.forEach(checkCol)
		rowIndex++
	}
	if (Match.Victory == "White") alert("White victory")
	if (Match.Victory == "Black") alert("Black victory")
	board = document.querySelector('[name="Board"]')
	rowIndex = 0
	Match.Board.forEach(checkRow)
}
// makeMove: sends a json with all the info about the move being made
function makeMove(moving) {
	_moving = JSON.stringify(moving)
	$.ajax({
		type: "POST",
		url: "Chess/Move",
		data: {_move: _moving },
		dataType: "json",
		complete: function() {
			requestMatch(currentMatchId)
		},
	});
}
function refreshMatch() {
	idToRequest = parseInt(document.querySelector('[name="IdDisplay"]').value)
	console.log(idToRequest)
	requestMatch(idToRequest)
}