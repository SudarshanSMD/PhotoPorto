function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("Text");
    if (data) {
        var childElement = document.getElementById(data);
        if (childElement) {
            if ($(ev.target).hasClass("droppable")) {           //Only allow drop inside the 2 divs
                ev.target.appendChild(childElement);
            }
            if ($(ev.target).hasClass("draggable")) {           //put in parent when dropped on draggable
                console.log('$(ev.target).parent()', $(ev.target).parent());
                $(ev.target).parent()[0].appendChild(childElement);
            }
            return false;
        }
    }
}