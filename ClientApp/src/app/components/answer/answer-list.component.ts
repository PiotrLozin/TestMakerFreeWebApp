import { Component, inject, Inject, Input, OnChanges, SimpleChanges } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { Question } from "../../interfaces/question";
import { Answer } from "../../interfaces/answer";

@Component({
  selector: "answer-list",
  templateUrl: './answer-list.component.html',
  styleUrls: ['./answer-list.component.css']
})

export class AnswerListComponent implements OnChanges {
  @Input() question: Question | undefined;
  answers: Answer[];
  title: string | undefined;
  baseUrl: string;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private router: Router) {

    this.baseUrl = "https://localhost:7136/";
    this.answers = [];
  }

  ngOnChanges(changes: SimpleChanges) {
    if (typeof changes['question'] !== "undefined") {

      // Download information under the key question
      var change = changes['question'];

      // Perform the task only if the value has changed
      if (!change.isFirstChange()) {

        // Perform http request and download result
        this.loadData();
      }
    }
  }

  loadData() {
    var url = this.baseUrl + "api/answer/All/" + this.question?.Id;
    this.http.get<Answer[]>(url).subscribe(result => {
      this.answers = result;
    }, error => console.error(error));
  }

  onCreate() {
    this.router.navigate(["/answer/create", this.question?.Id]);
  }

  onEdit(answer: Answer) {
    this.router.navigate(["/answer/edit", answer.Id]);
  }

  onDelete(answer: Answer) {
    if (confirm("Are you sure about deleting this answer?")) {
      var url = this.baseUrl + "api/answer/" + answer.Id;
      this.http
        .delete(url)
        .subscribe(res => {
          console.log("Answer " + answer.Id + " was deleted.");

          // Refresh list
          this.loadData();
        }, error => console.log(error));
    }
  }
}
