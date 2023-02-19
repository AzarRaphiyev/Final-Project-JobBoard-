let content = document.querySelector('.toggle-content')
$('.toggle-a').click(function (e) {
    console.log("salam")
    console.log(e.target)
    e.preventDefault();
    console.log(content)
    content.classList.toggle('show');
})