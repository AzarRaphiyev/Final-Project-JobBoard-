let deletebutton = document.querySelectorAll(".deleteItemImage");

deletebutton.forEach(btn => btn.addEventListener("click", function () {
    btn.parentElement.remove()
}))


  
let btn = document.querySelectorAll(".deletebtn");



btn.forEach(btn => btn.addEventListener("click", function (e) {

    e.preventDefault()


    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {

            let url = btn.getAttribute("href");

            fetch(url)
                .then(res => {

                    if (res.status == 200) {
                        window.location.reload(true)
                    } else {
                        alert("Item not found")
                    }
                })
        }
    })
}))
