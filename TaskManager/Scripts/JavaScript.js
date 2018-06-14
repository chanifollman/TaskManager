$(() => {
    $.connection.hub.start();
    const hub = $.connection.taskHub;

    $(".add-task").on('click', function () {
        const task = $("#task").val();
        hub.server.sendTask(task);
        $("#task").val('');
    })
    $(".table-div").on('click', "#doing-btn", function () {
        const userId = $("#user-id").val();
        const taskId = $(this).val();
        hub.server.sendId(userId, taskId);
    })

    $(".table-div").on('click', "#done-btn", function () {
        const taskId = $(this).val();
        hub.server.completeTask(taskId);
    })

    hub.client.appendTask = tasks => {
        $("#task-table tr:gt(0)").remove();
        tasks.forEach(task => {
            var taskString = `<tr><td>${task.Title}</td>`
            var id = $("#user-id").val();
            if (task.Status === 0) {
                var statusString = `<td><button class="btn btn-primary" id="doing-btn" value="${task.Id}">I'm doing this one!</button></td></tr>`
            }
            if (task.Status === 1) {
                if (parseInt(task.UserId) !== parseInt(id)) {
                    var statusString = `<td><button class="btn btn-warning" id="taken-btn">${task.Name} is doing this</button></td></tr>`
                }
                else {
                    var statusString = `<td><button class="btn btn-success" id="done-btn" value="${task.Id}">I'm Done</button></td></tr>`
                }
            }
            if (task.Status !== 2) {
                $("#task-table").append(taskString + statusString);
            }
        });

    }
})