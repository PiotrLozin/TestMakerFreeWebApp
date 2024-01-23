import { HttpClient } from "@angular/common/http";
import { Component, Inject } from "@angular/core";
import { Quiz } from "../../interfaces/quiz";

@Component({
  selector: "quiz-list",
  templateUrl: './quiz-list.component.html',
  styleUrls: ['./quiz-list.component.css']
})


export class QuizListComponent {
  title: string;
  selectedQuiz: Quiz | undefined;
  quizzes: Quiz[] | undefined;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.title = "newest quizes";
    var url = baseUrl + "api/quiz/Latest";
    http.get<Quiz[]>(url).subscribe(result => {
      this.quizzes = result;
    }, error => console.error(error));
  }

  onSelect(quiz: Quiz) {
    this.selectedQuiz = quiz;
    console.log("Selected was quiz with identificator: "
      + this.selectedQuiz.Id);
  }
}


